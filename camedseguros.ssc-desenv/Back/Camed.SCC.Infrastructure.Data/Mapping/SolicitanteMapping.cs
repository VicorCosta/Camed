using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SolicitanteMapping : IEntityTypeConfiguration<Solicitante>
    {
        public void Configure(EntityTypeBuilder<Solicitante> builder)
        {
            builder.ToTable("Solicitante");
            builder.HasKey(k => k.Id);

            //builder.Property(p => p.Usuario).IsRequired();
            builder.Property(p => p.Email).IsRequired();

            builder.HasOne(f => f.Usuario).WithMany().HasForeignKey("Usuario_Id");
        }
    }
}
