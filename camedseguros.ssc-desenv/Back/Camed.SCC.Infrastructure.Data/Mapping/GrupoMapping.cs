using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class GrupoMapping : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("Grupo");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Ativo).IsRequired();
            builder.Property(p => p.SempreVisualizarObservacao).IsRequired();
            builder.Property(p => p.AtribuirAtendente).IsRequired();
            builder.Property(p => p.AtribuirOperador).IsRequired();
            builder.Property(p => p.CancelarSolicitacao).IsRequired();

            builder.HasMany(m => m.SubGrupos).WithOne(o => o.Grupo).HasForeignKey(fk => fk.Grupo_Id);
        }
    }
}
