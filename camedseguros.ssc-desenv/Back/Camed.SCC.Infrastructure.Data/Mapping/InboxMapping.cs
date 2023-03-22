using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class InboxMapping : IEntityTypeConfiguration<Inbox>
    {
        public void Configure(EntityTypeBuilder<Inbox> builder)
        {
            builder.ToTable("Inbox");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Assunto).IsRequired();
            builder.Property(p => p.Texto);
            builder.HasOne(fk => fk.numeroSolicitacao).WithMany().HasForeignKey("Solicitacao_Id");
            builder.HasOne(f => f.UsuarioDestinatario).WithMany().HasForeignKey("UsuarioDestinatario_Id").IsRequired();
            builder.HasOne(f => f.UsuarioRemetente).WithMany().HasForeignKey("UsuarioRemetente_Id");
            builder.HasMany(m => m.Anexos).WithOne(o => o.Inbox).HasForeignKey("Inbox_Id");

        }
    }
}
