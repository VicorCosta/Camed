using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class VariaveisDeEmailMapping : IEntityTypeConfiguration<VariaveisDeEmail>
    {
        public void Configure(EntityTypeBuilder<VariaveisDeEmail> builder)
        {
            builder.ToTable("VariaveisDeEmail");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Parametro_Id);
        }
    }
}
