using AutoMapper;
using Camed.SCC.Infrastructure.CrossCutting.Identity;
using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.IoC;
using Camed.SSC.Application.Requests;
using Camed.SSC.Core;
using Camed.SSC.WebAPI.Filter;
using MediatR;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Camed.SSC.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static string ContentRootPath
        {
            get
            {
                var app = AppDomain.CurrentDomain;

                if (string.IsNullOrEmpty(app.RelativeSearchPath))
                {
                    return app.BaseDirectory;
                }

                return app.RelativeSearchPath;
            }
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(ContentRootPath, fileName);
            }
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options => 
            {
                options.Filters.Add(typeof(ApplicationExceptionFilter));
            })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1)
            .AddJsonOptions(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                o.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            services.AddOData();

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            services.AddDbContext<SSCContext>(options =>
            {
                options.UseSqlServer(ConnectionString.App);
            });

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton<ISigningConfigurations>(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(Configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddRouting(o => o.LowercaseUrls = true);
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("*");
            }));



            AddAuthorizationAndAuthentication(services, signingConfigurations, tokenConfigurations);
            services.AddMvcCore().AddJsonFormatters();

            new DependencyInjectionConfig(services).Register();

            AddSwagger(services);
            AddMediatr(services);
            AddAutoMapper(services);

            /*services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseMvc(routeBuilder => {
                routeBuilder.Expand().Select().OrderBy().Filter().SkipToken().MaxTop(100).Count();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSC");
                c.RoutePrefix = string.Empty;
            });

            loggerFactory.AddFile("Logs/log-{Date}.log");
            loggerFactory.AddEventSourceLogger();
            loggerFactory.AddDebug();
        }

        private static IEdmModel GetEdmModel()
        {
            try
            {
                var builder = new ODataConventionModelBuilder();
                builder.EnableLowerCamelCase();

                var entities = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                                .Where(x => x.FullName.Contains("Camed.SSC.Domain.Entities") 
                                                && !x.IsInterface 
                                                && !x.IsAbstract
                                                && !x.IsNested
                                                && !x.Name.Contains("<"))
                                .ToList();

                foreach (var entity in entities)
                {
                    MethodInfo mi = builder.GetType().GetMethod("EntitySet");
                    MethodInfo miConstructed = mi.MakeGenericMethod(entity);

                    miConstructed.Invoke(builder, new[] { entity.Name });
                }

                return builder.GetEdmModel();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        private static void AddMediatr(IServiceCollection services)
        {
            const string applicationAssemblyName = "Camed.SSC.Application";
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            services.AddScoped(typeof(IRequestPreProcessor<,>), typeof(RequestLogger<>));
            services.AddMediatR(assembly);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(new[]
            {
                AppDomain.CurrentDomain.Load("Camed.SCC.Infrastructure.CrossCutting"),
                AppDomain.CurrentDomain.Load("Camed.SSC.Application")
            });
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "SSC",
                        Version = "v1",
                        Description = "Sistema de Solicitação Camed",
                        Contact = new Contact
                        {
                            Name = "Camed",
                            Url = ""
                        }
                    });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(XmlCommentsFilePath);
                c.CustomSchemaIds(x => x.FullName);
        });
        }

        private static void AddAuthorizationAndAuthentication(IServiceCollection services, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
    }
}
