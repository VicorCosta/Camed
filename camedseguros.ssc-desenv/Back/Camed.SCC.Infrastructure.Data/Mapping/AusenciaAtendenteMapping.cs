using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AusenciaAtendenteMapping : IEntityTypeConfiguration<AusenciaAtendente>
    {
        public void Configure(EntityTypeBuilder<AusenciaAtendente> builder)
        {
            builder.ToTable("AusenciaDeAtendente");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.DataInicioAusencia).IsRequired();
            builder.Property(p => p.DataFinalAusencia).IsRequired();

            builder.HasOne(fk => fk.Atendente).WithMany().HasForeignKey("Atendente_Id").IsRequired();
        }
    }
}
