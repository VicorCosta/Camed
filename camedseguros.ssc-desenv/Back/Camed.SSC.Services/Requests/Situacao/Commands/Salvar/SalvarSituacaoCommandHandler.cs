using AutoMapper;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarSituacaoCommandHandler : HandlerBase, IRequestHandler<SalvarSituacaoCommand, IResult>

    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public SalvarSituacaoCommandHandler(IMapper mapper, IUnitOfWork uow)
        {
            this.mapper = mapper;
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarSituacaoCommand request, CancellationToken cancellationToken)
        {

            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            Domain.Entities.Situacao situacao = null;
            
            if (request.Id == 0)
            {
                situacao = mapper.Map<Domain.Entities.Situacao>(request);
               
                await uow.GetRepository<Domain.Entities.Situacao>().AddAsync(situacao);
            }
            else
            {
                situacao = await uow.GetRepository<Domain.Entities.Situacao>().GetByIdAsync(request.Id);
                situacao.Nome = request.Nome;
                situacao.Tipo = request.Tipo;
                situacao.TempoSLA = request.TempoSLA;
                situacao.EFimFluxo = request.EFimFluxo;
                situacao.PendenciaCliente = request.PendenciaCliente;

                uow.GetRepository<Domain.Entities.Situacao>().Update(situacao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }
    }
}
