using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SLAAgendamentoMapping : IEntityTypeConfiguration<SLAAgendamento>
    {
        public void Configure(EntityTypeBuilder<SLAAgendamento> builder)
        {
            builder.ToTable("SLAAgendamento");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Situacao);
            builder.Property(s => s.DataEHoraInicial).IsRequired();
            builder.Property(s => s.DataEHoraFinal).IsRequired();
                                             
        }
    }
}
