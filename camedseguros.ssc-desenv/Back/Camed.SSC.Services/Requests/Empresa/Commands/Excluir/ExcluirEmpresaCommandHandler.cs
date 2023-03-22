using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{

    public class ExcluirEmpresaCommandHandler : HandlerBase, IRequestHandler<ExcluirEmpresaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirEmpresaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }
            var acao = await uow.GetRepository<Domain.Entities.Empresa>().QueryFirstOrDefaultAsync(r => r.Id == request.Id, includes: new[] { "TiposDeSeguro" });


            if (acao != null)
            {
                if (acao.TiposDeSeguro.Any())
                {
                    throw new ApplicationException("Contem relação com tipo de seguro");
                }
                await uow.GetRepository<Domain.Entities.Empresa>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Empresa não localizada na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }
    }
}
