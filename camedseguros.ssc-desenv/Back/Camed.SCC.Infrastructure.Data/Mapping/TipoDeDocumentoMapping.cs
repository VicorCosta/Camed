using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeDocumentoMapping : IEntityTypeConfiguration<TipoDeDocumento>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TipoDeDocumento> builder)
        {
            builder.ToTable("TipoDeDocumento");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
        }
    }
}
