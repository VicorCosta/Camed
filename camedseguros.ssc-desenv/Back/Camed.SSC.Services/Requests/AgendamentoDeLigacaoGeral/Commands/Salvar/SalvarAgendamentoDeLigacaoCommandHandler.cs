using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgendamentoDeLigacaoGeralCommandHandler : HandlerBase, IRequestHandler<SalvarAgendamentoDeLigacaoGeralCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAgendamentoDeLigacaoGeralCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAgendamentoDeLigacaoGeralCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.AgendamentoDeLigacaoGeral acao = new Domain.Entities.AgendamentoDeLigacaoGeral();

                try
                {
                    var data = DateTime.ParseExact(request.DataAgendamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    acao.DataAgendamento = data;
                }
                catch
                {
                    throw new ApplicationException("Formato de data invalida - dd/MM/yyyy");
                }
                try
                {
                    var data = DateTime.ParseExact(request.DataLigacao, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    acao.DataLigacao = data;
                }
                catch
                {
                    throw new ApplicationException("Formato de data invalida - dd/MM/yyyy");
                }

                acao.Motivo = request.Motivo;
                var tipoRetorno = await uow.GetRepository<TipoRetornoLigacao>().GetByIdAsync(request.TipoRetornoLigacao);
                acao.TipoRetornoLigacao = tipoRetorno;

                await uow.GetRepository<Domain.Entities.AgendamentoDeLigacaoGeral>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.AgendamentoDeLigacaoGeral>().GetByIdAsync(request.Id) ?? throw new ApplicationException("Agendamento de ligacao nao encontrado");

                try
                {
                    var data = DateTime.ParseExact(request.DataAgendamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    acao.DataAgendamento = data;
                }
                catch
                {
                    throw new ApplicationException("Formato de data invalida - dd/MM/yyyy");
                }
                try
                {
                    var data = DateTime.ParseExact(request.DataLigacao, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    acao.DataLigacao = data;
                }
                catch
                {
                    throw new ApplicationException("Formato de data invalida - dd/MM/yyyy");
                }

                acao.Motivo = request.Motivo;
                var tipoRetorno = await uow.GetRepository<TipoRetornoLigacao>().GetByIdAsync(request.TipoRetornoLigacao);
                acao.TipoRetornoLigacao = tipoRetorno;
                acao.Solicitacao_Id = 1;
                await uow.GetRepository<Domain.Entities.AgendamentoDeLigacaoGeral>().AddAsync(acao);
                uow.GetRepository<Domain.Entities.AgendamentoDeLigacaoGeral>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
