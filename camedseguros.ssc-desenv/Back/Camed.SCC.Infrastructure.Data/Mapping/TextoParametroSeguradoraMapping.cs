using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TextoParametroSeguradoraMapping : IEntityTypeConfiguration<TextoParametroSeguradora>
    {
        public void Configure(EntityTypeBuilder<TextoParametroSeguradora> builder)
        {
            builder.ToTable("TextoParametroSeguradora");
            builder.HasKey(K => K.Id);

            builder.Property(p => p.Texto).IsRequired();

            builder.HasOne(fk => fk.Seguradora).WithMany().HasForeignKey("Seguradora_Id");
        }
    }
}
