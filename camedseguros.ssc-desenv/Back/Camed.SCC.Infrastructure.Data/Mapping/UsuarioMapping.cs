using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Matricula).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Login).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.CPF).IsRequired().HasMaxLength(12);
            builder.Property(p => p.Email);
            builder.Property(p => p.Ativo).IsRequired();
            builder.Property(p => p.Excluido).IsRequired();
            builder.Property(p => p.Senha).IsRequired();
            builder.Property(p => p.EnviarSLA).IsRequired();
            builder.Property(p => p.EhCalculista);

            builder.HasOne(o => o.Empresa).WithMany().HasForeignKey("Empresa_Id").IsRequired();
            builder.HasOne(o => o.Agencia).WithMany().HasForeignKey("Agencia_Id");
            builder.HasMany(o => o.Grupos).WithOne(o => o.Usuario).HasForeignKey(fk => fk.Usuario_Id);
        }
    }
}
