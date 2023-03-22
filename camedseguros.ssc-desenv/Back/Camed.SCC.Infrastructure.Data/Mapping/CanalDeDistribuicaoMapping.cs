using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class CanalDeDistribuicaoMapping : IEntityTypeConfiguration<CanalDeDistribuicao>
    {
        public void Configure(EntityTypeBuilder<CanalDeDistribuicao> builder)
        {
            builder.ToTable("CanalDeDistribuicao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(30).IsRequired();
            builder.Property(p => p.Ativo).IsRequired();

        }
    }
}
