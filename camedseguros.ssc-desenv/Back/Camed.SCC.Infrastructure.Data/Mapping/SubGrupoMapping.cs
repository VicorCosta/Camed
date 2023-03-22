using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping {
    public class SubGrupoMapping : IEntityTypeConfiguration<SubGrupo>
    {
        public void Configure(EntityTypeBuilder<SubGrupo> builder)
        {
            builder.ToTable("GrupoGrupo");
            builder.HasKey(k => new {k.Subgrupo_Id, k.Grupo_Id});

            builder.Property(p => p.Grupo_Id).HasColumnName("Grupo_Id");
            builder.Property(p => p.Subgrupo_Id).HasColumnName("Grupo_Id1");

            builder.HasOne(o => o.Subgrupo).WithMany().HasForeignKey(fk => fk.Subgrupo_Id);
        }
    }
}
