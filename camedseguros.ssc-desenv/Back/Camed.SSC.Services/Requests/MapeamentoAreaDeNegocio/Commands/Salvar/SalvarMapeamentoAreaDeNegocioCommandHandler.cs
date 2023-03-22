using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMapeamentoAreaDeNegocioCommandHandler : HandlerBase, IRequestHandler<SalvarMapeamentoAreaDeNegocioCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarMapeamentoAreaDeNegocioCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarMapeamentoAreaDeNegocioCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            if (request.Id == 0) {
                Domain.Entities.MapeamentoAreaDeNegocio mapeamentoNegocio = new Domain.Entities.MapeamentoAreaDeNegocio();                
                
                              

                var _tipoAgencia = uow.GetRepository<Domain.Entities.TipoDeAgencia>()
                    .GetByIdAsync((int)request.TipoDeAgencia_Id).Result;

                var _tipoProduto = uow.GetRepository<Domain.Entities.TipoDeProduto>()
                    .GetByIdAsync((int)request.TipoDeProduto_Id).Result;

                var _tipDeSeguro = uow.GetRepository<Domain.Entities.TipoDeSeguro>()
                    .GetByIdAsync((int)request.TipoDeSeguro_Id).Result;

                if(request.VinculoBNB_Id != null) {
                    var _vinculoBNB = uow.GetRepository<Domain.Entities.VinculoBNB>()
                    .GetByIdAsync((int)request.VinculoBNB_Id).Result;
                    mapeamentoNegocio.VinculoBNB = _vinculoBNB;
                }

                var _areaNegocio = uow.GetRepository<Domain.Entities.AreaDeNegocio>()
                    .GetByIdAsync((int)request.AreaDeNegocio_Id).Result;


                mapeamentoNegocio.TipoDeAgencia = _tipoAgencia;
                mapeamentoNegocio.TipoDeProduto = _tipoProduto;
                mapeamentoNegocio.TipoDeSeguro = _tipDeSeguro;
                mapeamentoNegocio.AreaDeNegocio = _areaNegocio;

                if(request.OperacaoDeFinanciamento != null) {
                    mapeamentoNegocio.OperacaoDeFinanciamento = request.OperacaoDeFinanciamento;   
                }
                await uow.GetRepository<Domain.Entities.MapeamentoAreaDeNegocio>().AddAsync(mapeamentoNegocio);

            } else {
                var _includes = new[] { "TipoDeSeguro", "TipoDeProduto", "VinculoBNB", "AreaDeNegocio" };
                var mapeamentoNegocio = await uow.GetRepository<Domain.Entities.MapeamentoAreaDeNegocio>().QueryFirstOrDefaultAsync(w => w.Id == request.Id, includes: _includes);

                var _tipoAgencia = uow.GetRepository<Domain.Entities.TipoDeAgencia>()
                    .GetByIdAsync((int)request.TipoDeAgencia_Id).Result;

                var _tipoProduto = uow.GetRepository<Domain.Entities.TipoDeProduto>()
                    .GetByIdAsync((int)request.TipoDeProduto_Id).Result;

                var _tipDeSeguro = uow.GetRepository<Domain.Entities.TipoDeSeguro>()
                    .GetByIdAsync((int)request.TipoDeSeguro_Id).Result;

                if(request.VinculoBNB_Id != null) {
                    var _vinculoBNB = uow.GetRepository<Domain.Entities.VinculoBNB>()
                        .GetByIdAsync((int)request.VinculoBNB_Id).Result;
                    mapeamentoNegocio.VinculoBNB = _vinculoBNB;
                } else {
                    mapeamentoNegocio.VinculoBNB = null;
                }

                var _areaNegocio = uow.GetRepository<Domain.Entities.AreaDeNegocio>()
                    .GetByIdAsync((int)request.AreaDeNegocio_Id).Result;

                mapeamentoNegocio.TipoDeAgencia = _tipoAgencia;
                mapeamentoNegocio.TipoDeProduto = _tipoProduto;
                mapeamentoNegocio.TipoDeSeguro = _tipDeSeguro;
                mapeamentoNegocio.AreaDeNegocio = _areaNegocio;

                mapeamentoNegocio.OperacaoDeFinanciamento = request.OperacaoDeFinanciamento;

                uow.GetRepository<Domain.Entities.MapeamentoAreaDeNegocio>().Update(mapeamentoNegocio);
            }

            await uow.CommitAsync();
            result.Successfully = true;
            return await Task.FromResult(result);
        }

    }
}
