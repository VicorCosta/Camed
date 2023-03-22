using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SituacaoMapping : IEntityTypeConfiguration<Situacao>
    {
        public void Configure(EntityTypeBuilder<Situacao> builder)
        {
            builder.ToTable("Situacao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Tipo).IsRequired().HasMaxLength(2);
            builder.Property(p => p.TempoSLA);
            builder.Property(p => p.EFimFluxo).IsRequired();
            builder.Property(p => p.PendenciaCliente);
        }
    }
}
