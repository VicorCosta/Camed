using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class ParametrosSistemaMapping : IEntityTypeConfiguration<ParametrosSistema>
    {
        public void Configure(EntityTypeBuilder<ParametrosSistema> builder)
        {
            builder.ToTable("ParametrosSistema");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Parametro).IsRequired();
            builder.Property(p => p.Valor).IsRequired();
            builder.Property(p => p.Tipo).IsRequired();

            builder.HasOne(fk => fk.TipoDeParametro).WithMany().HasForeignKey("TipoDeParametro_Id");
            builder.HasOne(fk => fk.VariaveisDeEmail).WithMany().HasForeignKey("VariaveisDeEmail_Id");
        }
    }
}
