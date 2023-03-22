using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AuditLogDetailMapping : IEntityTypeConfiguration<AuditLogDetail>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AuditLogDetail> builder)
        {
            builder.ToTable("VW_AUDITORIA_DETALHE");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ColumnName).IsRequired();
            builder.Property(p => p.OriginalValue).IsRequired();
            builder.Property(p => p.NewValue).IsRequired();
            builder.Property(p => p.AuditLogId).IsRequired();

            builder.HasOne(o => o.Auditoria).WithMany(m => m.AuditLogDetail).HasForeignKey("AuditLogId");
        }
    }
}
