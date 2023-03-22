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
    public class SalvarFeriadoCommandHandler : HandlerBase, IRequestHandler<SalvarFeriadoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarFeriadoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarFeriadoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Feriado acao = new Domain.Entities.Feriado();
                try
                {
                    var dataRequest = DateTime.Parse(request.Data, new CultureInfo("pt-BR"));
                    acao.Data = dataRequest.Date;
                }
                catch
                {
                    throw new ApplicationException("Formato de data invalida - dd/MM/yyyy");
                }

                acao.Descricao = request.Descricao;
                acao.Estado = request.Estado;
                var municio_bd = await uow.GetRepository<Cidade>().GetByIdAsync(request.Municipio);
                acao.Municipio = municio_bd ?? throw new ApplicationException("Municipio selecionado nao existe");
                acao.Pais = request.Pais;
              
                await uow.GetRepository<Domain.Entities.Feriado>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.Feriado>().GetByIdAsync(request.Id) ?? throw new ApplicationException("Feriado nao encontrado");

                var dataRequest = DateTime.Parse(request.Data, new CultureInfo("pt-BR"));
                acao.Data = dataRequest.Date;

                acao.Descricao = request.Descricao;
                acao.Estado = request.Estado;
                var municio_bd = await uow.GetRepository<Cidade>().GetByIdAsync(request.Municipio);
                acao.Municipio = municio_bd ?? throw new ApplicationException("Municipio selecionado nao existe");
                acao.Pais = request.Pais;
                uow.GetRepository<Domain.Entities.Feriado>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
