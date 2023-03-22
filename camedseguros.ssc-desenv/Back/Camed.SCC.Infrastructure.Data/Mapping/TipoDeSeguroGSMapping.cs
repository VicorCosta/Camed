using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeSeguroGSMapping : IEntityTypeConfiguration<TipoDeSeguroGS>
    {
        public void Configure(EntityTypeBuilder<TipoDeSeguroGS> builder)
        {
            builder.ToTable("VW_TIPO_SEGURO");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.nm_Abrev);
        }
    }
}
