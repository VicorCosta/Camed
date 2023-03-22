using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class ExpedienteMapping : IEntityTypeConfiguration<Expediente>
    {
        public void Configure(EntityTypeBuilder<Expediente> builder)
        {
            builder.ToTable("Expediente");
            builder.HasKey(p => p.Id);

            builder.Property(p=>p.Dia).IsRequired();

            builder.Property(p=>p.HoraInicialManha).IsRequired();

            builder.Property(p=>p.HoraFinalManha).IsRequired();

            builder.Property(p => p.HoraInicialTarde).IsRequired();

            builder.Property(p => p.HoraFinalTarde).IsRequired();

        }
    }
}
