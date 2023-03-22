using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class EmpresaTipoDeSeguroMapping : IEntityTypeConfiguration<EmpresaTipoDeSeguro>
    {
        public void Configure(EntityTypeBuilder<EmpresaTipoDeSeguro> builder)
        {
            builder.ToTable("EmpresaTipoDeSeguro");
            builder.HasKey(k => new { k.Empresa_id, k.TipoDeSeguro_id });

            builder.Property(p => p.Permitido_Abrir);

            builder.HasOne(p => p.Empresa).WithMany(m => m.TiposDeSeguro).HasForeignKey(fk=>fk.Empresa_id);
            builder.HasOne(p => p.TipoDeSeguro).WithMany(m => m.Empresas).HasForeignKey(fk => fk.TipoDeSeguro_id);
        }
    }
}
