using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SeguradoraMapping : IEntityTypeConfiguration<VW_SEGURADORA>
    {
        public void Configure(EntityTypeBuilder<VW_SEGURADORA> builder)
        {
            builder.ToTable("VW_SEGURADORA");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nm_Seguradora).IsRequired();
        }
    }
}
