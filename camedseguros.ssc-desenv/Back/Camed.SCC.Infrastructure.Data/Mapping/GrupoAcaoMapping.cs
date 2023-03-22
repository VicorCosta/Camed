using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class GrupoAcaoMapping : IEntityTypeConfiguration<GrupoAcao>
    {
        public void Configure(EntityTypeBuilder<GrupoAcao> builder)
        {
            builder.ToTable("GrupoAcao");
            builder.HasKey(k => new {k.Grupo_Id, k.Acao_Id});

            builder.HasOne(fk => fk.Grupo).WithMany(fk => fk.Acoes).HasForeignKey(fk => fk.Grupo_Id);
            builder.HasOne(fk => fk.Acao).WithMany(fk => fk.Grupos).HasForeignKey(fk => fk.Acao_Id);
        }
    }
}
