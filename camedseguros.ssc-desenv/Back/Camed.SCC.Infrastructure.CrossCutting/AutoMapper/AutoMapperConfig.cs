using AutoMapper;
using Camed.SCC.Infrastructure.CrossCutting.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace Camed.SCC.Infrastructure.CrossCutting
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ClaimsPrincipal, SSCIdentity>()
                    .BeforeMap((principal, identity) =>
                    {
                        identity.Id = Int32.Parse(principal.Claims.FirstOrDefault(f => f.Type == "Id")?.Value);
                        identity.Nome = principal.Claims.FirstOrDefault(f => f.Type == "Nome")?.Value ?? null;
                        identity.Login = principal.Claims.FirstOrDefault(f => f.Type == "Login")?.Value ?? null;
                        identity.Email = principal.Claims.FirstOrDefault(f => f.Type == "Email")?.Value ?? null;
                        identity.Matricula = principal.Claims.FirstOrDefault(f => f.Type == "Matricula")?.Value ?? null;
                        identity.CPF = principal.Claims.FirstOrDefault(f => f.Type == "CPF")?.Value ?? null;
                    });
        }
    }
}
