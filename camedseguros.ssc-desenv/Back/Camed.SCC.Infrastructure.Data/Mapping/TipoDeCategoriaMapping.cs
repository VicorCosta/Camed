using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeCategoriaMapping : IEntityTypeConfiguration<TipoDeCategoria>
    {
        public void Configure(EntityTypeBuilder<TipoDeCategoria> builder)
        {
            builder.ToTable("TipoDeCategoria");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Descricao).IsRequired().HasMaxLength(30);
            builder.HasOne(fk => fk.TipoDeProduto).WithMany().HasForeignKey("TipoDeProduto_Id");
        }
    }
}
