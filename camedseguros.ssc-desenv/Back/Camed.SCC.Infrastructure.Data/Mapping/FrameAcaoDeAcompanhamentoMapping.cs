using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class FrameAcaoDeAcompanhamentoMapping : IEntityTypeConfiguration<FrameAcaoDeAcompanhamento>
    { 
        public void Configure(EntityTypeBuilder<FrameAcaoDeAcompanhamento> builder)
        {
            builder.ToTable("FrameAcaoDeAcompanhamento");
            builder.HasKey(k => new { k.AcaoDeAcompanhamento_Id, k.Frame_Id });

            builder.HasOne(p => p.AcaoDeAcompanhamento).WithMany(m => m.Frames).HasForeignKey(fk => fk.AcaoDeAcompanhamento_Id);
            builder.HasOne(p => p.Frame).WithMany(m => m.AcoesAcompanhamento).HasForeignKey(fk => fk.Frame_Id);
        }
    }
  
}
