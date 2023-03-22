using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class GrupoDeProducaoMapping : IEntityTypeConfiguration<GrupoDeProducao>
    {
        public void Configure(EntityTypeBuilder<GrupoDeProducao> builder)
        {
            builder.ToTable("VW_GRUPODEPRODUCAO");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nome).IsRequired();
        }
    }
}
