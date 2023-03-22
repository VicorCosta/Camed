using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SolCheckListMapping : IEntityTypeConfiguration<SolCheckList>
    {
        public void Configure(EntityTypeBuilder<SolCheckList> builder)
        {
            builder.ToTable("SolCheckList");
            builder.HasKey(k => k.Id);

            /*builder.Property(p => p.Solicitacao).IsRequired();
            builder.Property(p => p.TipoDeDocumento).IsRequired();*/

            builder.HasOne(f => f.TipoDeDocumento).WithMany().HasForeignKey("TipoDeDocumento_Id");
            builder.HasOne(f => f.Solicitacao).WithMany().HasForeignKey("Solicitacao_Id");
        }
    }
}
