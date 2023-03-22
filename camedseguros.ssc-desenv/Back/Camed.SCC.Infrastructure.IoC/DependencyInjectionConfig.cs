using Camed.SCC.Infrastructure.Data;
using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.Services;
using Camed.SSC.Application.Util;
using Camed.SSC.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Camed.SCC.Infrastructure.IoC
{
    public class DependencyInjectionConfig
    {
        private readonly IServiceCollection services;

        public DependencyInjectionConfig(IServiceCollection services)
        {
            this.services = services;
        }


        public void Register()
        {
            services.AddDbContext<SSCContext>(options => options.UseSqlServer(ConnectionString.App));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IIdentityAppService, IdentityAppService>();

            services.AddScoped<IInboxAppService, InboxAppService>();
            services.AddScoped<ISolicitacaoAppService, SolicitacaoAppService>();
            services.AddScoped<IAcompanhamentoAppService, AcompanhamentoAppService>();
            services.AddScoped<IAreaDeNegocioAppService, AreaDeNegocioAppService>();
            services.AddScoped<IFrameAppService, FrameAppService>();
            services.AddScoped<IFuncionarioAppService, FuncionarioAppService>();
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<ICaracter, Caracter>();
        }
    }
}
