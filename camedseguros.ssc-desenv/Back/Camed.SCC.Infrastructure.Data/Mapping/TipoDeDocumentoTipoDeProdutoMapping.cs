using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeDocumentoTipoDeProdutoMapping : IEntityTypeConfiguration<TipoDeDocumentoTipoDeProduto> {
        public void Configure(EntityTypeBuilder<TipoDeDocumentoTipoDeProduto> builder) {
            builder.ToTable("TipoDeDocumentoTipoDeProduto");
            builder.HasKey(k => new { k.TipoDeDocumento_Id, k.TipoDeProduto_Id });

            builder.HasOne(p => p.TipoDeDocumento).WithMany(m => m.TiposDeProduto).HasForeignKey(fk => fk.TipoDeDocumento_Id);
            builder.HasOne(p => p.TipoDeProduto).WithMany(m => m.TiposDeDocumento).HasForeignKey(fk => fk.TipoDeProduto_Id);
        }
    }

}
