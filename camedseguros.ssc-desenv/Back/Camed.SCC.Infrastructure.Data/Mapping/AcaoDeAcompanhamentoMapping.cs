using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AcaoDeAcompanhamentoMapping : IEntityTypeConfiguration<AcaoDeAcompanhamento>
    {
        public void Configure(EntityTypeBuilder<AcaoDeAcompanhamento> builder)
        {
            builder.ToTable("AcaoDeAcompanhamento");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired();
        }
    }
}
