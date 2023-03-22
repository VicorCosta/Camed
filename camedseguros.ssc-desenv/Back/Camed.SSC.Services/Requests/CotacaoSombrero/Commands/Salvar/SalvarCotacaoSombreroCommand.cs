using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCotacaoSombreroCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}
        public int CepAreaDeRisco { get; set; }
        public int CodigoProduto { get; set; }
        public string NomeProduto { get; set; }
        public int CodigoCultivo { get; set; }
        public string NomeCultivo { get; set; }
        public int NivelCobertura { get; set; }
        public decimal? AreaTotal { get; set; }
        public string TipoCotacao { get; set; }
        public string UnidadePesoCultivo { get; set; }
        public decimal ValorCusteio_Preco { get; set; }
        public string TipoSubvencao { get; set; }
        public int? NumeroCotacao { get; set; }
        public string MunicipioPropriedade { get; set; }
        public decimal? ImportanciaSeguradaHectare { get; set; }
        public decimal? ImportanciaSegurada { get; set; }
        public decimal? ProdutividadeEsperada { get; set; }
        public decimal? ProdutividadeSegurada { get; set; }
        public decimal? PremioTotal { get; set; }
        public decimal? ValorSubvencaoFederal { get; set; }
        public decimal? ValorSubvencaoEstatual { get; set; }
        public decimal? ValorPremioSegurado { get; set; }
        public int? ParcelamentoMaximo { get; set; }
        public DateTime? DataCotacao { get; set; }
        public DateTime? DataValidadeCotacao { get; set; }
        public string PDFCotacao { get; set; }
        public string NomeDaPropriedade { get; set; }
        public string EnderecoDaPropriedade { get; set; }
        public string NumeroDaPropriedade { get; set; }
        public string BairroDaPropriedade { get; set; }
        public string CepDaPropriedade { get; set; }
        public string RepresentanteLegalCPF { get; set; }
        public string RepresentanteLegalNome { get; set; }
        public string RepresentanteLegalTelefone { get; set; }
        public DateTime? DataInicioPlantio { get; set; }
        public DateTime? DataFimPlantio { get; set; }
        public string CoberturaQualidade { get; set; }
        public DateTime? DataPrimeiraParcela { get; set; }
        public int? ParcelamentoEscolhido { get; set; }
        public string NumeroProposta { get; set; }
        public DateTime? Data_processamento { get; set; }
        public string Observacao { get; set; }
        public string IdTransacao { get; set; }
        public DateTime? DataTransacao { get; set; }
        public string Resumo { get; set; }
        public string Status { get; set; }
        public string RecusaDescricao { get; set; }
        public string Erros { get; set; }
        public string RetornoDaPropostaSombreroJSON { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarCotacaoSombreroValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
