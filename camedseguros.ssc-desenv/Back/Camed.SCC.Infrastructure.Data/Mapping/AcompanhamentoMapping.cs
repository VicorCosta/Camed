using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AcompanhamentoMapping : IEntityTypeConfiguration<Acompanhamento>
    {
        public void Configure(EntityTypeBuilder<Acompanhamento> builder)
        {
            builder.ToTable("Acompanhamento");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Ordem).IsRequired();
            builder.Property(p => p.DataEHora).IsRequired();
            builder.Property(p => p.PermiteVisualizarAnexo).IsRequired();
            builder.Property(p => p.PermiteVisualizarObservacao).IsRequired();
            builder.Property(p => p.Situacao_Id);
            builder.Property(p => p.Solicitacao_Id);

            builder.HasOne(f => f.Situacao).WithMany().HasForeignKey("Situacao_Id");
            builder.HasOne(f => f.Solicitacao).WithMany().HasForeignKey("Solicitacao_Id");
            builder.HasOne(f => f.Grupo).WithMany().HasForeignKey("Grupo_Id");
            builder.HasOne(f => f.Atendente).WithMany().HasForeignKey("Atendente_Id");

            builder.HasMany(f => f.Anexos).WithOne().HasForeignKey("Acompanhamento_Id");
            builder.HasMany(f => f.VendasCompartilhadas).WithOne().HasForeignKey("Acompanhamento_Id");
        }
    }
}
