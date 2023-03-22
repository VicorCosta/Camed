using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarParametroSistemaCommandHandler : HandlerBase, IRequestHandler<SalvarParametroSistemaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarParametroSistemaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarParametroSistemaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            Domain.Entities.ParametrosSistema acao = await uow.GetRepository<Domain.Entities.ParametrosSistema>().GetByIdAsync(request.Id) ?? new Domain.Entities.ParametrosSistema();
            acao.TipoDeParametro_Id = request.TipoDeParametro_Id;
            acao.VariaveisDeEmail_Id = request.VariaveisDeEmail_Id;
            acao.Parametro = request.Parametro;
            acao.Tipo = request.Tipo;
            acao.Valor = request.Valor;

            if (request.Id == 0)
            {
                await uow.GetRepository<Domain.Entities.ParametrosSistema>().AddAsync(acao);
            }
            else
            {
                uow.GetRepository<Domain.Entities.ParametrosSistema>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
