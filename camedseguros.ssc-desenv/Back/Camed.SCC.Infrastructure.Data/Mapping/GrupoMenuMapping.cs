using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class GrupoMenuMapping : IEntityTypeConfiguration<GrupoMenu>
    { 
        public void Configure(EntityTypeBuilder<GrupoMenu> builder)
        {
            builder.ToTable("GrupoMenu");
            builder.HasKey(k => new { k.Grupo_Id, k.Menu_Id });

            builder.HasOne(p => p.Grupo).WithMany(m => m.Menus).HasForeignKey(fk => fk.Grupo_Id);
            builder.HasOne(p => p.Menu).WithMany(m => m.Grupos).HasForeignKey(fk => fk.Menu_Id);
        }
    }
  
}
