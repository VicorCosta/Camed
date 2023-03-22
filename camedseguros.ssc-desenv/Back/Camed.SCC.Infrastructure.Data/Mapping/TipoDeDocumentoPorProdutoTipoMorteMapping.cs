using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping {
    class TipoDeDocumentoPorProdutoTipoMorteMapping : IEntityTypeConfiguration<TipoDeDocumentoTipoDeProdutoTipoMorte> {
        public void Configure(EntityTypeBuilder<TipoDeDocumentoTipoDeProdutoTipoMorte> builder) {
            builder.ToTable("TipoDeDocumentoPorProdutoTipoMorte");

            builder.HasKey(k => new { k.TipoDeDocumento_Id, k.TipoDeProduto_Id, k.Id, k.TipoMorte_Id});
            builder.Property(k => k.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.TipoDeDocumento).WithMany(m => m.TiposDeProdutoMorte).HasForeignKey(fk => fk.TipoDeDocumento_Id);

            builder.HasOne(p => p.TipoDeProduto).WithMany(m => m.TiposDeProdutoMorte).HasForeignKey(fk => fk.TipoDeProduto_Id);

            builder.HasOne(p => p.TipoMorte).WithMany(m => m.TiposDeProdutoMorte).HasForeignKey(fk => fk.TipoMorte_Id);
        }
    }
}
