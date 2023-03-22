using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MenuAcaoMapping : IEntityTypeConfiguration<MenuAcao>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MenuAcao> builder)
        {
            builder.ToTable("MenuAcao");
            builder.HasKey(p => p.id_menu_acao);

            builder.HasOne(p => p.Menu).WithMany(m => m.MenuAcao);
            builder.HasOne(p => p.Acao).WithMany(m => m.MenuAcaos).HasForeignKey(fk => fk.acao_id);

        }
    }
}
