using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeSeguroTipoDeProdutoMapping : IEntityTypeConfiguration<TipoDeSeguroTipoDeProduto>
    { 
        public void Configure(EntityTypeBuilder<TipoDeSeguroTipoDeProduto> builder)
        {
            builder.ToTable("TipoDeSeguroTipoDeProduto");
            builder.HasKey(k => new { k.TipoDeSeguro_Id, k.TipoDeProduto_Id });

            builder.HasOne(p => p.TipoDeSeguro).WithMany(m => m.TiposDeProduto).HasForeignKey(fk => fk.TipoDeSeguro_Id);
            builder.HasOne(p => p.TipoDeProduto).WithMany(m => m.TiposDeSeguro).HasForeignKey(fk => fk.TipoDeProduto_Id);
        }
    }
  
}
