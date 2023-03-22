using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MenuMapping : IEntityTypeConfiguration<Menu>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menu");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Label).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Rota).HasMaxLength(100);
            builder.Property(p => p.Icone).HasMaxLength(50);
            builder.Property(p => p.Ajudatexto);

            builder.HasOne(m => m.Superior).WithMany(w => w.Submenus).HasForeignKey("Menu_Id");
            builder.HasMany(m => m.MenuAcao).WithOne(w => w.Menu).HasForeignKey(a => a.menu_id);
        }
    }
}
