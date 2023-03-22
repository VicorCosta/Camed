using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class UsuarioGrupoAgenciaMapping : IEntityTypeConfiguration<UsuarioGrupoAgencia>
    { 
        public void Configure(EntityTypeBuilder<UsuarioGrupoAgencia> builder)
        {
            builder.ToTable("UsuarioGrupoAgencia");
            builder.HasKey(k => new { k.Usuario_Id, k.GrupoAgencia_Id });

            builder.HasOne(p => p.Usuario).WithMany(m => m.GruposAgencias).HasForeignKey(fk => fk.Usuario_Id);
            builder.HasOne(p => p.GrupoAgencia).WithMany(m => m.Usuarios).HasForeignKey(fk => fk.GrupoAgencia_Id);
        }
    }
}
