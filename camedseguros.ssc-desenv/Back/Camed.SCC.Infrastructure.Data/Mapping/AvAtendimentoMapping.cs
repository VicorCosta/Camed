using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AvAtendimentoMapping : IEntityTypeConfiguration<AvAtendimento>
    {
        public void Configure(EntityTypeBuilder<AvAtendimento> builder)
        {
            builder.ToTable("AvaliacaoDeSolicitacao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Observacao).HasMaxLength(500);
            builder.Property(p => p.Nota).IsRequired();
            builder.Property(p => p.Solicitacao_Id).IsRequired();
            builder.Property(p => p.DataAvaliacao).IsRequired();
            builder.Property(p => p.Usuario_Id).IsRequired();
        }
    }
}
