using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class GrupoDeAgenciaMapping : IEntityTypeConfiguration<GrupoAgencia>
    {
        public void Configure(EntityTypeBuilder<GrupoAgencia> builder)
        {
            builder.ToTable("GrupoAgencia");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(20);
        }
    }
}
