using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class RamoMapping : IEntityTypeConfiguration<Ramo>
    {
        public void Configure(EntityTypeBuilder<Ramo> builder)
        {
            builder.ToTable("VW_RAMO");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nm_Ramo).IsRequired();

            builder.HasOne(f => f.Seguradora).WithMany().HasForeignKey("Seguradora_Id");
        }
    }
}
