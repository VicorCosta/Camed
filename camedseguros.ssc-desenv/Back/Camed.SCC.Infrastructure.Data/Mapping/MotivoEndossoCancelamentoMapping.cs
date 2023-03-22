using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MotivoEndossoCancelamentoMapping : IEntityTypeConfiguration<MotivoEndossoCancelamento>
    {
        public void Configure(EntityTypeBuilder<MotivoEndossoCancelamento> builder)
        {
            builder.ToTable("MotivoEndossoCancelamento");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Descricao);
        }
    }
}
