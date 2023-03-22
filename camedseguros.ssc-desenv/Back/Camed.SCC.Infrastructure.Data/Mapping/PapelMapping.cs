using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class PapelMapping : IEntityTypeConfiguration<Papel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Papel> builder)
        {
            builder.ToTable("Papel");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao).IsRequired();
            builder.Property(p => p.Nome).IsRequired();

            builder.HasMany(p => p.Acoes).WithOne(p => p.Papel);
        }
    }
}
