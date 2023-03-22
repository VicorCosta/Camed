using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTextoParametroSeguradoraCommandHandler : HandlerBase, IRequestHandler<SalvarTextoParametroSeguradoraCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarTextoParametroSeguradoraCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarTextoParametroSeguradoraCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var seguradora = await uow.GetRepository<Domain.Entities.TextoParametroSeguradora>().GetByIdAsync(request.Id) ?? new Domain.Entities.TextoParametroSeguradora();
            seguradora.Seguradora_Id = request.Seguradora_Id;
            seguradora.Texto = request.Texto;


            if (request.Id == 0)
            {
                await uow.GetRepository<Domain.Entities.TextoParametroSeguradora>().AddAsync(seguradora);

            }
            else
            {
                uow.GetRepository<Domain.Entities.TextoParametroSeguradora>().Update(seguradora);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
