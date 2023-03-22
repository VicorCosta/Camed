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

    public class ExcluirMenuCommandHandler : HandlerBase, IRequestHandler<ExcluirMenuCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ExcluirMenuCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirMenuCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var acao = await uow.GetRepository<Domain.Entities.Menu>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "MenuAcao", "MenuAcao.Acao" });

            if (acao != null)
            {
                if (acao.MenuAcao.Count() != 0)
                {
                    var lista = acao.MenuAcao.Select(s => s.Acao.Descricao);
                    throw new ApplicationException($"Menu contém relação com a(s) ação/açôes: '{string.Join(", ", lista)}'");
                }

                await uow.GetRepository<Domain.Entities.Menu>().RemoveAndSaveAsync(acao);
            }
            else
            {
                result.Message = "Menu não localizado na base de dados";
                return await Task.FromResult(result);
            }

            return await Task.FromResult(result);
        }

    }
}
