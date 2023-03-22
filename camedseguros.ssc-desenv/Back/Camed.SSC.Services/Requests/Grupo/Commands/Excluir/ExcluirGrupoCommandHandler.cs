using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests {

    public class ExcluirGrupoCommandHandler : HandlerBase, IRequestHandler<ExcluirGrupoCommand, IResult> {
        private readonly IUnitOfWork uow;

        public ExcluirGrupoCommandHandler(IUnitOfWork uow) {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirGrupoCommand request, CancellationToken cancellationToken) {
            try {
                if(!request.IsValid()) {
                    base.result.SetValidationErros(request);
                    return await Task.FromResult(result);
                }

                var acao = await uow.GetRepository<Domain.Entities.Grupo>().GetByIdAsync(request.Id);

                if(acao != null) {
                    await uow.GetRepository<Domain.Entities.Grupo>().RemoveAndSaveAsync(acao);

                } else {
                    result.Message = $"Error! O Grupo não foi localizado no banco de dados.";
                    result.Successfully = false;
                    return await Task.FromResult(result);
                }

                result.Message = $"{acao.Nome} foi deletado!";
                return await Task.FromResult(result);
            } catch(Exception e) {

                if(e.InnerException.Message != null) {

                    var msgAlert = "";

                    if(e.InnerException.Message.Contains("\"dbo.GrupoMenu\"")) {
                        msgAlert = "Error! O Grupo não pode ser deletado porque é refência na tabela Grupo Menu.";

                    } else if(e.InnerException.Message.Contains("\"dbo.GrupoGrupo\"")) {
                        msgAlert = "Error! O Grupo não pode ser deletado porque é subgrupo de outro grupo.";
                    } else {
                        msgAlert = "Error!";
                    };

                    result.Message = msgAlert;
                    result.Successfully = false;
                    return await Task.FromResult(result);

                }

                var msg = "";

                if(!String.IsNullOrEmpty(e.InnerException.Message))
                    msg = e.InnerException.Message;
                else
                    msg = e.Message;

                throw new ApplicationException("Erro: " + msg);

            }

        }
    }
}
