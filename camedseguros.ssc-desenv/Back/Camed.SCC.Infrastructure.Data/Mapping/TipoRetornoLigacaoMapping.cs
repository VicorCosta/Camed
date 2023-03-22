using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class TipoRetornoLigacaoMapping : IEntityTypeConfiguration<TipoRetornoLigacao>
    {
        public void Configure(EntityTypeBuilder<TipoRetornoLigacao> builder)
        {
            builder.ToTable("TipoRetornoLigacao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Descricao).HasMaxLength(100).IsRequired();
        }
    }
}
