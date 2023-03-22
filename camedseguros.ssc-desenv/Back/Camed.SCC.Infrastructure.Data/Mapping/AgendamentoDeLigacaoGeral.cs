using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AgendamentoDeLigacaoGeralMapping : IEntityTypeConfiguration<AgendamentoDeLigacaoGeral>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AgendamentoDeLigacaoGeral> builder)
        {
            builder.ToTable("AgendamentoDeLigacaoGeral");
            //builder.HasKey(p => p.Id);

            builder.Property(p => p.DataAgendamento).IsRequired();
            builder.Property(p => p.DataLigacao);
            builder.Property(p => p.Motivo).IsRequired().HasMaxLength(200);
            builder.Property(p => p.numero).IsRequired().HasMaxLength(4);

           // builder.HasOne(fk => fk.TipoRetornoLigacao).WithMany().HasForeignKey("TipoRetornoLigacao_Id");
        }
    }
}
