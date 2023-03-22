using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCotacaoSombreroCommandHandler : HandlerBase, IRequestHandler<SalvarCotacaoSombreroCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarCotacaoSombreroCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarCotacaoSombreroCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            Domain.Entities.CotacaoSombrero acao;
            if (request.Id == 0)
            {
                acao = new Domain.Entities.CotacaoSombrero();
                acao.CepAreaDeRisco = request.CepAreaDeRisco;

                acao.CodigoProduto = request.CodigoProduto;
                acao.NomeProduto = request.NomeProduto;
                acao.CodigoCultivo = request.CodigoCultivo;
                acao.NomeCultivo = request.NomeCultivo;
                acao.NivelCobertura = request.NivelCobertura;
                acao.AreaTotal = request.AreaTotal;
                acao.TipoCotacao = request.TipoCotacao;
                acao.UnidadePesoCultivo = request.UnidadePesoCultivo;
                acao.ValorCusteio_Preco = request.ValorCusteio_Preco;
                acao.TipoSubvencao = request.TipoSubvencao;
                acao.NumeroCotacao = request.NumeroCotacao;
                acao.MunicipioPropriedade = request.MunicipioPropriedade;
                acao.ImportanciaSeguradaHectare = request.ImportanciaSeguradaHectare;
                acao.ImportanciaSegurada = request.ImportanciaSegurada;
                acao.ProdutividadeEsperada = request.ProdutividadeEsperada;
                acao.ProdutividadeSegurada = request.ProdutividadeSegurada;
                acao.PremioTotal = request.PremioTotal;
                acao.ValorSubvencaoFederal = request.ValorSubvencaoFederal;
                acao.ValorSubvencaoEstatual = request.ValorSubvencaoEstatual;
                acao.ValorPremioSegurado = request.ValorPremioSegurado;
                acao.ParcelamentoMaximo = request.ParcelamentoMaximo;
                acao.DataCotacao = request.DataCotacao;
                acao.DataValidadeCotacao = request.DataValidadeCotacao;
                acao.PDFCotacao = request.PDFCotacao;
                acao.NomeDaPropriedade = request.NomeDaPropriedade;
                acao.EnderecoDaPropriedade = request.EnderecoDaPropriedade;
                acao.NumeroDaPropriedade = request.NumeroDaPropriedade;
                acao.BairroDaPropriedade = request.BairroDaPropriedade;
                acao.CepDaPropriedade = request.CepDaPropriedade;
                acao.RepresentanteLegalCPF = request.RepresentanteLegalCPF;
                acao.RepresentanteLegalNome = request.RepresentanteLegalNome;
                acao.RepresentanteLegalTelefone = request.RepresentanteLegalTelefone;
                acao.DataInicioPlantio = request.DataInicioPlantio;
                acao.DataFimPlantio = request.DataFimPlantio;
                acao.CoberturaQualidade = request.CoberturaQualidade;
                acao.DataPrimeiraParcela = request.DataPrimeiraParcela;
                acao.ParcelamentoEscolhido = request.ParcelamentoEscolhido;
                acao.NumeroProposta = request.NumeroProposta;
                acao.Data_processamento = request.Data_processamento;
                acao.Observacao = request.Observacao;
                acao.IdTransacao = request.IdTransacao;
                acao.DataTransacao = request.DataTransacao;
                acao.Resumo = request.Resumo;
                acao.Status = request.Status;
                acao.RecusaDescricao = request.RecusaDescricao;
                acao.Erros = request.Erros;
                acao.RetornoDaPropostaSombreroJSON = request.RetornoDaPropostaSombreroJSON;
 

                await uow.GetRepository<Domain.Entities.CotacaoSombrero>().AddAsync(acao);

            }
            else
            {
                acao = await uow.GetRepository<Domain.Entities.CotacaoSombrero>().GetByIdAsync(request.Id);

                acao.CepAreaDeRisco = request.CepAreaDeRisco;
                acao.CodigoProduto = request.CodigoProduto;
                acao.NomeProduto = request.NomeProduto;
                acao.CodigoCultivo = request.CodigoCultivo;
                acao.NomeCultivo = request.NomeCultivo;
                acao.NivelCobertura = request.NivelCobertura;
                acao.AreaTotal = request.AreaTotal;
                acao.TipoCotacao = request.TipoCotacao;
                acao.UnidadePesoCultivo = request.UnidadePesoCultivo;
                acao.ValorCusteio_Preco = request.ValorCusteio_Preco;
                acao.TipoSubvencao = request.TipoSubvencao;
                acao.NumeroCotacao = request.NumeroCotacao;
                acao.MunicipioPropriedade = request.MunicipioPropriedade;
                acao.ImportanciaSeguradaHectare = request.ImportanciaSeguradaHectare;
                acao.ImportanciaSegurada = request.ImportanciaSegurada;
                acao.ProdutividadeEsperada = request.ProdutividadeEsperada;
                acao.ProdutividadeSegurada = request.ProdutividadeSegurada;
                acao.PremioTotal = request.PremioTotal;
                acao.ValorSubvencaoFederal = request.ValorSubvencaoFederal;
                acao.ValorSubvencaoEstatual = request.ValorSubvencaoEstatual;
                acao.ValorPremioSegurado = request.ValorPremioSegurado;
                acao.ParcelamentoMaximo = request.ParcelamentoMaximo;
                acao.DataCotacao = request.DataCotacao;
                acao.DataValidadeCotacao = request.DataValidadeCotacao;
                acao.PDFCotacao = request.PDFCotacao;
                acao.NomeDaPropriedade = request.NomeDaPropriedade;
                acao.EnderecoDaPropriedade = request.EnderecoDaPropriedade;
                acao.NumeroDaPropriedade = request.NumeroDaPropriedade;
                acao.BairroDaPropriedade = request.BairroDaPropriedade;
                acao.CepDaPropriedade = request.CepDaPropriedade;
                acao.RepresentanteLegalCPF = request.RepresentanteLegalCPF;
                acao.RepresentanteLegalNome = request.RepresentanteLegalNome;
                acao.RepresentanteLegalTelefone = request.RepresentanteLegalTelefone;
                acao.DataInicioPlantio = request.DataInicioPlantio;
                acao.DataFimPlantio = request.DataFimPlantio;
                acao.CoberturaQualidade = request.CoberturaQualidade;
                acao.DataPrimeiraParcela = request.DataPrimeiraParcela;
                acao.ParcelamentoEscolhido = request.ParcelamentoEscolhido;
                acao.NumeroProposta = request.NumeroProposta;
                acao.Data_processamento = request.Data_processamento;
                acao.Observacao = request.Observacao;
                acao.IdTransacao = request.IdTransacao;
                acao.DataTransacao = request.DataTransacao;
                acao.Resumo = request.Resumo;
                acao.Status = request.Status;
                acao.RecusaDescricao = request.RecusaDescricao;
                acao.Erros = request.Erros;
                acao.RetornoDaPropostaSombreroJSON = request.RetornoDaPropostaSombreroJSON;

                uow.GetRepository<Domain.Entities.CotacaoSombrero>().Update(acao);
            }

            await uow.CommitAsync();
            
            result.Payload = acao.Id;
            return await Task.FromResult(result);
        }

    }
}
