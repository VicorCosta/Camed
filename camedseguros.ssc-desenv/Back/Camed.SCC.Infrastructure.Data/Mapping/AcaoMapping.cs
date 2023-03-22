using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AcaoMapping : IEntityTypeConfiguration<Acao>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Acao> builder)
        {
            builder.ToTable("Acao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao).IsRequired();
            builder.Property(p => p.Nome).IsRequired();

            builder.HasOne(f => f.Papel).WithMany().HasForeignKey("Papel_id");
        }
    }
}
