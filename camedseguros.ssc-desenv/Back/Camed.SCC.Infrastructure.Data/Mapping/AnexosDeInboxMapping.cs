using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AnexosDeInboxMapping : IEntityTypeConfiguration<AnexosDeInbox>
    {
        public void Configure(EntityTypeBuilder<AnexosDeInbox> builder)
        {
            builder.ToTable("AnexosDeInbox");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Caminho).IsRequired();

            builder.HasOne(f => f.Inbox).WithMany(w => w.Anexos).HasForeignKey("Inbox_Id");
        }
    }
}
