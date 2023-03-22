using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgendamentoDeLigacaoCommandHandler : HandlerBase, IRequestHandler<SalvarAgendamentoDeLigacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarAgendamentoDeLigacaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarAgendamentoDeLigacaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.AgendamentoDeLigacao acao = new Domain.Entities.AgendamentoDeLigacao();
                var solicitacao = await uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(w => w.Numero == request.NSolicitacao);
                var tipoRetorno = await uow.GetRepository<TipoRetornoLigacao>().GetByIdAsync(request.TipoRetornoLigacao);
                try
                {
                    var data = DateTime.ParseExact(request.DataAgendamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    acao.DataAgendamento = data;
                }
                catch
                {
                    throw new ApplicationException("Formato de data invalida - dd/MM/yyyy");
                }

                bool motivoInvalido = !String.IsNullOrEmpty(request.Motivo) && Char.IsLetter(request.Motivo[0]);

                if (solicitacao == null)
                {
                    throw new ApplicationException("Número de Solicitação inválido. Verifique, e tente novamente.");
                }

                if (!motivoInvalido)
                {
                    throw new ApplicationException("Motivo tem formato inválido. Verifique, e tente novamente.");
                }

                acao.Motivo = request.Motivo;
                acao.Solicitacao = solicitacao;
                acao.TipoRetornoLigacao = tipoRetorno;

                await uow.GetRepository<Domain.Entities.AgendamentoDeLigacao>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.AgendamentoDeLigacao>().GetByIdAsync(request.Id) ?? throw new ApplicationException("Agendamento de ligacao nao encontrado");

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
                var solicitacao = await uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(w => w.Numero == request.NSolicitacao);
                acao.Solicitacao = solicitacao;
                var tipoRetorno = await uow.GetRepository<TipoRetornoLigacao>().GetByIdAsync(request.TipoRetornoLigacao);
                acao.TipoRetornoLigacao = tipoRetorno;

                await uow.GetRepository<Domain.Entities.AgendamentoDeLigacao>().AddAsync(acao);
                uow.GetRepository<Domain.Entities.AgendamentoDeLigacao>().Update(acao);
            }

            await uow.CommitAsync();

            
            return await Task.FromResult(result);
        }

    }
}
