using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoMorteMapping : IEntityTypeConfiguration<TipoMorte>
    {
        public void Configure(EntityTypeBuilder<TipoMorte> builder)
        {
            builder.ToTable("TipoMorte");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao).IsRequired().HasMaxLength(100);
        }
    }
}
