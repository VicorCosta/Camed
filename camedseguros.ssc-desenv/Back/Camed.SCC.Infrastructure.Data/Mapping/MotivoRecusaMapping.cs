using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MotivoRecusaMapping : IEntityTypeConfiguration<MotivoRecusa>
    {
        public void Configure(EntityTypeBuilder<MotivoRecusa> builder)
        {
            builder.ToTable("MotivoRecusa");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Ativo);
        }
    }
}
