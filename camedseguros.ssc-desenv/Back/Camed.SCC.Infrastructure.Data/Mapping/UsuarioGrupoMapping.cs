using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class UsuarioGrupoMapping : IEntityTypeConfiguration<UsuarioGrupo>
    { 
        public void Configure(EntityTypeBuilder<UsuarioGrupo> builder)
        {
            builder.ToTable("UsuarioGrupo");
            builder.HasKey(k => new { k.Usuario_Id, k.Grupo_Id });

            builder.HasOne(p => p.Usuario).WithMany(m => m.Grupos).HasForeignKey(fk => fk.Usuario_Id);
            builder.HasOne(p => p.Grupo).WithMany().HasForeignKey(fk => fk.Grupo_Id);
        }
    }
}
