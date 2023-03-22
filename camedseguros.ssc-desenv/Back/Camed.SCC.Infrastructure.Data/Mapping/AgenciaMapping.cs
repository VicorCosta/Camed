using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AgenciaMapping : IEntityTypeConfiguration<Agencia>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Agencia> builder)
        {
            builder.ToTable("Agencia");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Codigo);
            builder.Property(p => p.Nome);
            builder.Property(p => p.SuperId);
            builder.Property(p => p.Super);
        }
    }
}
