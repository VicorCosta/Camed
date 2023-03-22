using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TiposDeParametros : IEntityTypeConfiguration<TiposdeParametros>
    {
        public void Configure(EntityTypeBuilder<TiposdeParametros> builder)
        {
            builder.ToTable("TiposDeParametros");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(200);
        }
    }
}
