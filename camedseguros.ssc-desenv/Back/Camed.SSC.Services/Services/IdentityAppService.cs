using AutoMapper;
using Camed.SCC.Infrastructure.CrossCutting.Identity;
using Camed.SSC.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Camed.SSC.Application.Services
{
    public class IdentityAppService : IIdentityAppService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IMapper mapper;
        private static SSCIdentity instance;

        public IdentityAppService(IHttpContextAccessor httpContext, IMapper mapper)
        {
            this.httpContext = httpContext;
            this.mapper = mapper;
        }

        public SSCIdentity Identity
        {
            get
            {
                if(instance == null)
                {
                    instance = mapper.Map<ClaimsPrincipal, SSCIdentity>(httpContext.HttpContext.User);
                }

                return instance;
            }
        }
    }
}
