using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class UsuarioAreaDeNegocioMapping : IEntityTypeConfiguration<UsuarioAreaDeNegocio>
    { 
        public void Configure(EntityTypeBuilder<UsuarioAreaDeNegocio> builder)
        {
            builder.ToTable("UsuarioAreaDeNegocio");
            builder.HasKey(k => new { k.Usuario_Id, k.AreaDeNegocio_Id });

            builder.HasOne(p => p.Usuario).WithMany(m => m.AreasDeNegocio).HasForeignKey(fk => fk.Usuario_Id);
            builder.HasOne(p => p.AreaDeNegocio).WithMany(m => m.Usuarios).HasForeignKey(fk => fk.AreaDeNegocio_Id);
        }
    }
}
