using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AreaDeNegocioMapping : IEntityTypeConfiguration<AreaDeNegocio>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AreaDeNegocio> builder)
        {
            builder.ToTable("AreaDeNegocio");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired();
        }
    }
}
