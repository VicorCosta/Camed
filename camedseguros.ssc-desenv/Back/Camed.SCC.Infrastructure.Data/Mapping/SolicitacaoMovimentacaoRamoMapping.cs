using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SolicitacaoMovimentacaoRamoMapping : IEntityTypeConfiguration<SolicitacaoMovimentacaoRamo>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoMovimentacaoRamo> builder)
        {
            builder.ToTable("SolicitacaoMovimentacaoRamo");
            builder.HasKey(k => k.Id);

            builder.HasOne(fk => fk.Atendente).WithMany().HasForeignKey("Atendente_Id");
            builder.HasOne(fk => fk.Solicitacao).WithMany().HasForeignKey("Solicitacao_Id");
        }
    }
}
