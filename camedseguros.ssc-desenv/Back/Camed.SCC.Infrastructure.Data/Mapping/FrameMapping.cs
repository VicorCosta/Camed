using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class FrameMapping : IEntityTypeConfiguration<Frame>
    {
        public void Configure(EntityTypeBuilder<Frame> builder)
        {
            builder.ToTable("Frame");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(100);
        }
    }
}
