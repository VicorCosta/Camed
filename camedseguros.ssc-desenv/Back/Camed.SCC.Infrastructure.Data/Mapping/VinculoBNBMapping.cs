using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class VinculoBNBMapping : IEntityTypeConfiguration<VinculoBNB>
    {
        public void Configure(EntityTypeBuilder<VinculoBNB> builder)
        {
            builder.ToTable("VinculoBNB");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(100);
        }
    }
}
