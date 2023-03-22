using Camed.SCC.Infrastructure.CrossCutting.Identity;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Core.Security;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class LoginCommandHandler : HandlerBase, IRequestHandler<LoginCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly ISigningConfigurations signingConfigurations;

        public LoginCommandHandler(IUnitOfWork uow, ISigningConfigurations signingConfigurations)
        {
            this.uow = uow;
            this.signingConfigurations = signingConfigurations;
        }

        public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var senha = Encryption.CreateMD5Hash(request.Password);
            var user = await uow.GetRepository<Usuario>()
                                .QueryFirstOrDefaultAsync(w => w.Login == request.UserName && w.Senha == senha && w.Ativo == true,
                                        includes: new[] { "Agencia", "Empresa", "AreasDeNegocio", "AreasDeNegocio.AreaDeNegocio", "Grupos", "Grupos.Grupo", "GruposAgencias", "GruposAgencias.GrupoAgencia" });

            if (user != null)
            {
                var dataCriacao = DateTime.Now;
                var dataExpiracao = new DateTime(dataCriacao.Year, dataCriacao.Month, dataCriacao.Day, 23, 59, 59);
                var solicitante = await uow.GetRepository<Domain.Entities.Solicitante>().QueryFirstOrDefaultAsync(w => w.Id == user.Id);

                var identity = new ClaimsIdentity(new GenericIdentity(user.Login, "Login"),
                       new[]
                       {
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Login", user.Login),
                            new Claim("Nome", user.Nome),
                            new Claim("Email", user.Email),
                            new Claim("Matricula", user.Matricula),
                            new Claim("Grupos", user.Grupos != null ? string.Join(",", user.Grupos.Select(s => s.Grupo.Nome)) : null),
                            new Claim("AreasDeNegocio", user.AreasDeNegocio != null ? string.Join(",", user.AreasDeNegocio.Select(s => s.AreaDeNegocio.Id )) : null),
                            new Claim("EhCalculista", user.EhCalculista.ToString().ToLower()),
                            new Claim("EhSolicitante", user.EhSolicitante.ToString().ToLower()),
                            new Claim("EhAtendente", user.EhAtendente.ToString().ToLower()),
                            new Claim("EhAgrosul", user.EhAgrosul.ToString().ToLower()),
                            new Claim("PodeVisualizarObservacoes", user.PodeVisualizarObservacoes.ToString().ToLower()),
                            new Claim("PermitidoGerarCotacao", user.PermitidoGerarCotacao.ToString().ToLower()),
                            new Claim("GruposAgencias", user.GruposAgencias != null ? string.Join(",", user.GruposAgencias.Select(s => s.GrupoAgencia.Nome)) : null),
                            new Claim("TelefonePrincipal", solicitante == null || solicitante.TelefonePrincipal == null ? "" : solicitante.TelefonePrincipal),
                            new Claim("TelefoneCelular", solicitante == null || solicitante.TelefoneCelular == null ? "" : solicitante.TelefoneCelular),
                            new Claim("TelefoneAdicional", solicitante == null || solicitante.TelefoneAdicional == null ? "" : solicitante.TelefoneAdicional),
                            new Claim("Solicitante_id", solicitante == null ? "0" : solicitante.Id.ToString()),
                            new Claim("DataExpiracaoToken", dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss")),
                       }
                   );

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = AppConstants.Issuer,
                    Audience = AppConstants.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                result.Payload = new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    user = user
                };
            }
            else
            {
                throw new ApplicationException("Usuário e/ou senha incorreto.");
            }

            return await Task.FromResult(result);
        }
    }
}
