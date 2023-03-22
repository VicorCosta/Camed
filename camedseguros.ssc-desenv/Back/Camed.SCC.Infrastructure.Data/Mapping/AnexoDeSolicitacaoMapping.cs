using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AnexoDeSolicitacaoMapping : IEntityTypeConfiguration<AnexoDeSolicitacao>
    {
        public void Configure(EntityTypeBuilder<AnexoDeSolicitacao> builder)
        {
            builder.ToTable("AnexoDeSolicitacao");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Caminho).IsRequired();

            //builder.HasOne(fk => fk.Solicitacao).WithMany(fk => fk.Anexos).HasForeignKey("Solicitacao_Id");
        }
    }
}
