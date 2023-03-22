using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeProdutoMapping : IEntityTypeConfiguration<TipoDeProduto>
    {
        public void Configure(EntityTypeBuilder<TipoDeProduto> builder)
        {
            builder.ToTable("TipoDeProduto");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DescricaoSasParaTipoDeProduto).HasMaxLength(100);
            builder.Property(p => p.ObservacaoTipoDeProduto).HasMaxLength(8000);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Ativo).IsRequired();
            builder.Property(p => p.SlaMaximo).IsRequired();
            builder.Property(p => p.UsoInterno).IsRequired();
            builder.Property(p => p.Situacao_Id);

            builder.HasOne(fk => fk.Situacao).WithMany().HasForeignKey("Situacao_Id");
            builder.HasOne(fk => fk.SituacaoRenovacao).WithMany().HasForeignKey("SituacaoRenovacao_Id");

        }
    }
}
