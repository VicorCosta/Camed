using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirVinculoBNBCommandHandler : HandlerBase, IRequestHandler<ExcluirVinculoBNBCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirVinculoBNBCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirVinculoBNBCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (!request.IsValid())
                {
                    base.result.SetValidationErros(request);
                    return await Task.FromResult(result);
                }

                var acao = await uow.GetRepository<Domain.Entities.VinculoBNB>().GetByIdAsync(request.Id);

                if (acao != null)
                {
                    await uow.GetRepository<Domain.Entities.VinculoBNB>().RemoveAndSaveAsync(acao);
                }
                else
                {
                    result.Message = "Vinculo BNB não localizada na base de dados";
                    return await Task.FromResult(result);
                }

                
                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                var result = e;
                throw e;
            }

        }

    }
}
