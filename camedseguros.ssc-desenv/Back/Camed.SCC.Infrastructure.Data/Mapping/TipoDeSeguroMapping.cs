using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeSeguroMapping : IEntityTypeConfiguration<TipoDeSeguro>
    {
        public void Configure(EntityTypeBuilder<TipoDeSeguro> builder)
        {
            builder.ToTable("TipoDeSeguro");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(100);

            builder.HasOne(o => o.GrupoAgencia).WithMany().HasForeignKey("GrupoAgencia_Id").IsRequired();
        }

    }
}