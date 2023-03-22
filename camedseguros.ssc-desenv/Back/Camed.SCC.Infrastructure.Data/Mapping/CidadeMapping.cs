using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class CidadeMapping : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ToTable("VW_CIDADE");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(30).IsRequired();
            builder.Property(p => p.UF).HasMaxLength(2).IsRequired();
        }
    }
}
