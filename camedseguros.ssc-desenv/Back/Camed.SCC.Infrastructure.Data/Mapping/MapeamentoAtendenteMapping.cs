using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MapeamentoAtendenteMapping : IEntityTypeConfiguration<MapeamentoAtendente>
    {
        public void Configure(EntityTypeBuilder<MapeamentoAtendente> builder)
        {
            builder.ToTable("MapeamentoAtendente");
            builder.HasKey(k => k.Id);

            builder.HasOne(fk => fk.TipoDeSeguro).WithMany().HasForeignKey("TipoDeSeguro_Id");
            builder.HasOne(fk => fk.GrupoAgencia).WithMany().HasForeignKey("GrupoAgencia_Id");
            builder.HasOne(fk => fk.Atendente).WithMany().HasForeignKey("Atendente_Id");
            builder.HasOne(fk => fk.AreaDeNegocio).WithMany().HasForeignKey("AreaDeNegocio_Id");
            builder.HasOne(fk => fk.Agencia).WithMany().HasForeignKey("Agencia_Id");
        }
    }
}
