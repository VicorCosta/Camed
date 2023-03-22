using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AnexoDeAcompanhamentoMapping : IEntityTypeConfiguration<AnexoDeAcompanhamento>
    {
        public void Configure(EntityTypeBuilder<AnexoDeAcompanhamento> builder)
        {
            builder.ToTable("AnexoDeAcompanhamento");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Caminho).IsRequired();
        }
    }
}
