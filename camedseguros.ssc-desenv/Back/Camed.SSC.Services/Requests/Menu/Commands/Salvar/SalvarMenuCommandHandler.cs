using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMenuCommandHandler : HandlerBase, IRequestHandler<SalvarMenuCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarMenuCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarMenuCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0)
            {
                Domain.Entities.Menu menu = new Domain.Entities.Menu();
                menu.Label = request.Label;
                menu.Rota = request.Rota;
                menu.Icone = request.Icone;
                menu.Ajudatexto = request.Ajuda;

                if (menu.MenuAcao != null)
                {
                    menu.MenuAcao.UpdateCollection(request.Acoes, keyProperty: "acao_id");
                }


                if (request.MenuSuperior.HasValue)
                {
                    var menuSuperior = await uow.GetRepository<Domain.Entities.Menu>().GetByIdAsync(request.MenuSuperior.Value) ?? throw new ApplicationException("Menu não foi encontrado na base de dados");
                    menu.Superior = menuSuperior;
                }

                await uow.GetRepository<Domain.Entities.Menu>().AddAsync(menu);
            }
            else
            {
                var menu = await uow.GetRepository<Domain.Entities.Menu>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: new[] { "MenuAcao", "MenuAcao.Acao" });
                menu.Label = request.Label;
                menu.Rota = request.Rota;
                menu.Icone = request.Icone;
                menu.Ajudatexto = request.Ajuda;
                if (request.MenuSuperior.HasValue)
                {
                    var menuSuperior = await uow.GetRepository<Domain.Entities.Menu>().GetByIdAsync(request.MenuSuperior.Value) ?? throw new ApplicationException("Menu não foi encontrado na base de dados");
                    menu.Superior = menuSuperior;
                }

                menu.MenuAcao.UpdateCollection(request.Acoes, keyProperty: "acao_id");


                uow.GetRepository<Domain.Entities.Menu>().Update(menu);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
