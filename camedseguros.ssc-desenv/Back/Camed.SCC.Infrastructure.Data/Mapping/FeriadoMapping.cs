using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class FeriadoMapping : IEntityTypeConfiguration<Feriado>
    {
        public void Configure(EntityTypeBuilder<Feriado> builder)
        {
            builder.ToTable("Feriado");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Data).IsRequired();
            builder.Property(p => p.Pais).HasMaxLength(2);
            builder.Property(p => p.Estado).HasMaxLength(2);
            builder.Property(p => p.Descricao).IsRequired();

            builder.HasOne(o => o.Municipio).WithMany().HasForeignKey("Municipio_Id");
        }
    }
}
