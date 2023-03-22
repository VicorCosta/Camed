using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TextoParametrosSistemaMapping : IEntityTypeConfiguration<TextoParametrosSistema>
    {
        public void Configure(EntityTypeBuilder<TextoParametrosSistema> builder)
        {
            builder.ToTable("TextoParametrosSistema");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Texto).IsRequired();

            builder.HasOne(fk => fk.TipoDeProduto).WithMany().HasForeignKey("TipoDeProduto_Id");
            builder.HasOne(fk => fk.TipoDeSeguro).WithMany().HasForeignKey("TipoDeSeguro_Id");
        }
    }
}
