using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class CheckinMapping : IEntityTypeConfiguration<Checkin>
    {
        public void Configure(EntityTypeBuilder<Checkin> builder)
        {
            builder.ToTable("Checkin");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Latitude).IsRequired();
            builder.Property(p => p.Longitude).IsRequired();
            builder.Property(p => p.Endereco).IsRequired();
            builder.Property(p => p.Localidade).IsRequired();
            builder.Property(p => p.DataHora);

            builder.HasOne(f => f.Usuario).WithMany().HasForeignKey("Usuario_Id");
            builder.HasOne(f => f.Solicitacao).WithMany(f => f.Checkins).HasForeignKey("Solicitacao_Id");
        }
    }
}
