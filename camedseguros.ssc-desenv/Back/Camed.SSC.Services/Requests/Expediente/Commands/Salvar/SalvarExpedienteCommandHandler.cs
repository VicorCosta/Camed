using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarExpedienteCommandHandler : HandlerBase,IRequestHandler<SalvarExpedienteCommand, IResult>
                                   
    {
        private readonly IUnitOfWork uow;

        public SalvarExpedienteCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IResult> Handle(SalvarExpedienteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var obj = await uow.GetRepository<Domain.Entities.Expediente>().QueryFirstOrDefaultAsync(w => w.Dia == request.Dia);

            if (request.Id == 0)
            {
                Domain.Entities.Expediente expediente = new Domain.Entities.Expediente();
               
                if (obj == null)
                {
                    expediente.Dia = request.Dia;
                    expediente.HoraFinalManha = request.HoraFinalManhaDate;
                    expediente.HoraFinalTarde = request.HoraFinalTardeDate;
                    expediente.HoraInicialManha = request.HoraInicialManhaDate;
                    expediente.HoraInicialTarde = request.HoraInicialTardeDate;

                    await uow.GetRepository<Domain.Entities.Expediente>().AddAsync(expediente);
                }
                else
                {
                    throw new ApplicationException("O dia selecionado já está na base de dados. Se desejar edite as informações já existentes.");
                }
            }
            else
            {
                var expediente = await uow.GetRepository<Domain.Entities.Expediente>().GetByIdAsync(request.Id);
                if (obj == null || obj.Id == request.Id)
                {
                    expediente.Dia = request.Dia;
                    expediente.HoraFinalManha = request.HoraFinalManhaDate;
                    expediente.HoraFinalTarde = request.HoraFinalTardeDate;
                    expediente.HoraInicialManha = request.HoraInicialManhaDate;
                    expediente.HoraInicialTarde = request.HoraInicialTardeDate;

                    uow.GetRepository<Domain.Entities.Expediente>().Update(expediente);
                }
                else
                {
                    throw new ApplicationException("Ja existe esse dia");
                }
                
            }

            await uow.CommitAsync();
            return await Task.FromResult(result);
        }
    }
}
