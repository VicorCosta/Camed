using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAusenciaAtendenteCommandHandler : HandlerBase, IRequestHandler<SalvarAusenciaAtendenteCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAusenciaAtendenteCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAusenciaAtendenteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.AusenciaAtendente acao = new Domain.Entities.AusenciaAtendente();
                acao.Atendente_Id = request.Atendente_Id.Value;
                acao.DataInicioAusencia = request.DataInicioAusencia;
                acao.DataFinalAusencia = request.DataFinalAusencia;

                await uow.GetRepository<Domain.Entities.AusenciaAtendente>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.AusenciaAtendente>().GetByIdAsync(request.Id);

                acao.Atendente_Id = request.Atendente_Id.Value;
                acao.DataInicioAusencia = request.DataInicioAusencia;
                acao.DataFinalAusencia = request.DataFinalAusencia;

                uow.GetRepository<Domain.Entities.AusenciaAtendente>().Update(acao);
                }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
