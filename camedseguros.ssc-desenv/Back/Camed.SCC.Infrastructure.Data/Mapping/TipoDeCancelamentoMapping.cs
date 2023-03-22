using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoDeCancelamentoMapping : IEntityTypeConfiguration<TipoDeCancelamento>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TipoDeCancelamento> builder)
        {
            builder.ToTable("TipoDeCancelamento");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Descricao).IsRequired().HasMaxLength(50);
            builder.HasOne(fk => fk.GrupoAgencia).WithMany().HasForeignKey("GrupoAgencia_Id");
        }
    }
}
