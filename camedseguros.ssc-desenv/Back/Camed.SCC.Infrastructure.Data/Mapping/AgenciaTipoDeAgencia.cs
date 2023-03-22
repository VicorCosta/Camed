using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class AgenciaTipoDeAgenciaMapping : IEntityTypeConfiguration<AgenciaTipoDeAgencia>
    { 
        public void Configure(EntityTypeBuilder<AgenciaTipoDeAgencia> builder)
        {
            builder.ToTable("AgenciaTipoDeAgencia");
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Agencia).WithMany(m => m.TiposDeAgencias).HasForeignKey("Agencia_Id");
            builder.HasOne(p => p.TipoDeAgencia).WithMany(m => m.Agencias).HasForeignKey("TipoDeAgencia_Id");
        }
    }
  
}
