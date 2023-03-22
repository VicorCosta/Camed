using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AgendamentoDeLigacaoMapping : IEntityTypeConfiguration<AgendamentoDeLigacao>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AgendamentoDeLigacao> builder)

        {
            builder.ToTable("AgendamentoDeLigacao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DataAgendamento).IsRequired();
            builder.Property(p => p.DataLigacao);
            builder.Property(p => p.Motivo).IsRequired();

            builder.HasOne(fk => fk.TipoRetornoLigacao).WithMany().HasForeignKey("TipoRetornoLigacao_Id");
            builder.HasOne(fk => fk.Solicitacao).WithMany().HasForeignKey("Solicitacao_Id").IsRequired();
            

        }
    }
}
