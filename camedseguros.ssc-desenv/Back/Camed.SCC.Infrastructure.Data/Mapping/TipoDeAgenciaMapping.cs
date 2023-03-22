using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeAgenciaMapping : IEntityTypeConfiguration<TipoDeAgencia>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TipoDeAgencia> builder)
        {
            builder.ToTable("TipoDeAgencia");
            builder.HasKey(p => p.Id);
            builder.HasOne(fk => fk.GrupoAgencia).WithMany().HasForeignKey("GrupoAgencia_Id");
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(20);
        }
    }
}
