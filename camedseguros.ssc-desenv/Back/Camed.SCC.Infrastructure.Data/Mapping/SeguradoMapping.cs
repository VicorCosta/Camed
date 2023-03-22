using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SeguradoMapping : IEntityTypeConfiguration<Segurado>
    {
        public void Configure(EntityTypeBuilder<Segurado> builder)
        {
            builder.ToTable("Segurado");
            builder.HasKey(p => p.Id);


            builder.Property(p => p.Nome).IsRequired().HasMaxLength(400);
            builder.Property(p => p.CpfCnpj).IsRequired().HasMaxLength(28);
            builder.Property(p => p.Email).HasMaxLength(400);
            builder.Property(p => p.TelefonePrincipal).HasMaxLength(80);
            builder.Property(p => p.TelefoneCelular).HasMaxLength(40);
            builder.Property(p => p.TelefoneAdicional).HasMaxLength(40);
            builder.Property(p => p.EmailSecundario).HasMaxLength(400);
            builder.Property(p => p.Contato).HasMaxLength(50);

            builder.HasOne(fk => fk.VinculoBNB).WithMany().HasForeignKey("VinculoBNB_Id");

        }

    }
}
