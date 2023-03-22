using Camed.SCC.Infrastructure.Data.Interfaces;
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

    public class ExcluirTipoDeDocumentoCommandHandler : HandlerBase, IRequestHandler<ExcluirTipoDeDocumentoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirTipoDeDocumentoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirTipoDeDocumentoCommand request, CancellationToken cancellationToken)
        {

                if(!request.IsValid()) {
                    base.result.SetValidationErros(request);
                    return await Task.FromResult(result);
                }

                var acao = await uow.GetRepository<Domain.Entities.TipoDeDocumento>().QueryFirstOrDefaultAsync(w => w.Id == request.Id);
                var checkRelacao = await uow.GetRepository<Domain.Entities.TipoDeDocumentoTipoDeProdutoTipoMorte>().QueryFirstOrDefaultAsync(w => w.TipoDeDocumento_Id == request.Id);
                var SolCheckLists = uow.GetRepository<SolCheckList>().QueryFirstOrDefaultAsync(w => w.TipoDeDocumento.Id == request.Id);

                if(checkRelacao != null)
                {
                    throw new ApplicationException("Este tipo de documento possui vínculo com outras telas. Desmarque os campos 'Ramos de Seguro', 'Tipo de Morte', 'Ordem', 'Obrigatório', 'Ativo' e tente novamente.");
                } else
                {
                    acao.Excluido = true;
                    await uow.GetRepository<Domain.Entities.TipoDeDocumento>().UpdateAndSaveAsync(acao);
            }

                result.Message = "Documento deletado";
                return await Task.FromResult(result);
        }

    }
}
