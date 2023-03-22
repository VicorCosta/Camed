using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SolicitacaoIndicacoesMapping : IEntityTypeConfiguration<SolicitacaoIndicacoes>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoIndicacoes> builder)
        {
            builder.ToTable("SolicitacaoIndicacoes");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Telefone).IsRequired();

            builder.HasOne(p => p.Solicitacao).WithMany(p => p.Indicacoes).HasForeignKey("Solicitacao_Id");
        }
    }
}
