using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SequencialDeSolicitacaoMapping : IEntityTypeConfiguration<SequencialDeSolicitacao>
    {
        public void Configure(EntityTypeBuilder<SequencialDeSolicitacao> builder)
        {
            builder.ToTable("SequencialDeSolicitacao");
            builder.HasKey(k => k.Id);

            //builder.Property(p => p.Operador).IsRequired();

            builder.HasOne(fk => fk.Operador).WithMany().HasForeignKey("Operador_Id");
        }
    }
}
