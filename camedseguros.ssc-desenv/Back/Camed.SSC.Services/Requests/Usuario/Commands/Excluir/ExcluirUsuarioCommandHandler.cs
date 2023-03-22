using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirUsuarioCommandHandler : HandlerBase, IRequestHandler<ExcluirUsuarioCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService appIdentity;

        public ExcluirUsuarioCommandHandler(IUnitOfWork uow, IIdentityAppService appIdentity)
        {
            this.uow = uow;
            this.appIdentity = appIdentity;
        }

        public async Task<IResult> Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.IsValid())
                {
                    base.result.SetValidationErros(request);
                    return await Task.FromResult(result);
                }

                var usuario = await uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "AreasDeNegocio", "Grupos", "GruposAgencias" }) ?? throw new ApplicationException("Usuário não localizado na base de dados");

                if (appIdentity.Identity.Login == usuario.Login)
                {
                    throw new ApplicationException("Usuário não pode excluir seu próprio cadastro");
                }

                /*if (usuario.AreasDeNegocio.Any())
                {
                    usuario.AreasDeNegocio.Clear();
                }

                if (usuario.Grupos.Any())
                {
                    usuario.Grupos.Clear();
                }

                if (usuario.GruposAgencias.Any())
                {
                    usuario.GruposAgencias.Clear();
                }*/

                usuario.Excluido = true;

                await uow.GetRepository<Domain.Entities.Usuario>().UpdateAndSaveAsync(usuario);

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Erro: " + e);
            }
        }

    }
}
