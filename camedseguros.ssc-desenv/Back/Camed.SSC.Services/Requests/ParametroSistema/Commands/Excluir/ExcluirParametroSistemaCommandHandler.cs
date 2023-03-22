using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests { 

    public class ExcluirParametroSistemaCommandHandler : HandlerBase, IRequestHandler<ExcluirParametroSistemaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirParametroSistemaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirParametroSistemaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var teste = request.Id;
            
           /* var mapeamento = await uow.GetRepository<Domain.Entities.MapeamentoAcaoSituacao>().QueryFirstOrDefaultAsync(w => w.ParametrosSistema.Id == request.Id, includes: "ParametrosSistema");

            if (mapeamento != null)
            {
                throw new ApplicationException("Não é permitido remover esse paramêtro");
            }*/

            var acao = await uow.GetRepository<Domain.Entities.ParametrosSistema>().GetByIdAsync(request.Id);

            if (acao != null)
            {
                await uow.GetRepository<Domain.Entities.ParametrosSistema>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Mapeamento de Atendente não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }

    }
}
