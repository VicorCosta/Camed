using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class FormaDePagamentoMapping : IEntityTypeConfiguration<FormaDePagamento>
    {
        public void Configure(EntityTypeBuilder<FormaDePagamento> builder)
        {
            builder.ToTable("VW_FORMA_PGTO");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Ds_Forma).IsRequired();
        }
    }
}
