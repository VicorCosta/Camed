using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class CotacaoSombreroMapping : IEntityTypeConfiguration<CotacaoSombrero>
    {
        public void Configure(EntityTypeBuilder<CotacaoSombrero> builder)
        {
            builder.ToTable("CotacaoSombrero");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CepAreaDeRisco);
            builder.Property(p => p.CodigoProduto);
            builder.Property(p => p.NomeProduto);
            builder.Property(p => p.CodigoCultivo);
            builder.Property(p => p.NomeCultivo);
            builder.Property(p => p.NivelCobertura);
            builder.Property(p => p.AreaTotal);
            builder.Property(p => p.TipoCotacao);
            builder.Property(p => p.UnidadePesoCultivo);
            builder.Property(p => p.ValorCusteio_Preco);
            builder.Property(p => p.TipoSubvencao);
            builder.Property(p => p.NumeroCotacao);
            builder.Property(p => p.MunicipioPropriedade);
            builder.Property(p => p.ImportanciaSeguradaHectare);
            builder.Property(p => p.ImportanciaSegurada);
            builder.Property(p => p.ProdutividadeEsperada);
            builder.Property(p => p.ProdutividadeSegurada);
            builder.Property(p => p.PremioTotal);
            builder.Property(p => p.ValorSubvencaoFederal);
            builder.Property(p => p.ValorSubvencaoEstatual);
            builder.Property(p => p.ValorPremioSegurado);
            builder.Property(p => p.ParcelamentoMaximo);
            builder.Property(p => p.DataCotacao);
            builder.Property(p => p.DataValidadeCotacao);
            builder.Property(p => p.PDFCotacao);
            builder.Property(p => p.NomeDaPropriedade);
            builder.Property(p => p.EnderecoDaPropriedade);
            builder.Property(p => p.NumeroDaPropriedade);
            builder.Property(p => p.BairroDaPropriedade);
            builder.Property(p => p.CepDaPropriedade);
            builder.Property(p => p.RepresentanteLegalCPF);
            builder.Property(p => p.RepresentanteLegalNome);
            builder.Property(p => p.RepresentanteLegalTelefone);
            builder.Property(p => p.DataInicioPlantio);
            builder.Property(p => p.DataFimPlantio);
            builder.Property(p => p.CoberturaQualidade);
            builder.Property(p => p.DataPrimeiraParcela);
            builder.Property(p => p.ParcelamentoEscolhido);
            builder.Property(p => p.NumeroProposta);
            builder.Property(p => p.Data_processamento);
            builder.Property(p => p.Observacao);
            builder.Property(p => p.IdTransacao);
            builder.Property(p => p.DataTransacao);
            builder.Property(p => p.Resumo);
            builder.Property(p => p.Status);
            builder.Property(p => p.RecusaDescricao);
            builder.Property(p => p.Erros);
            builder.Property(p => p.RetornoDaPropostaSombreroJSON);

        }
    }
}
