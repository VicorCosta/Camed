using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MapeamentoAreaDeNegocioMapping : IEntityTypeConfiguration<MapeamentoAreaDeNegocio>
    {
        public void Configure(EntityTypeBuilder<MapeamentoAreaDeNegocio> builder)
        {
            builder.ToTable("MapeamentoAreaDeNegocio");
            builder.HasKey(k => k.Id);

            builder.HasOne(fk => fk.TipoDeAgencia).WithMany().HasForeignKey("TipoDeAgencia_Id");
            builder.HasOne(fk => fk.TipoDeProduto).WithMany().HasForeignKey("TipoDeProduto_Id");
            builder.HasOne(fk => fk.TipoDeSeguro).WithMany().HasForeignKey("TipoDeSeguro_Id");
            builder.HasOne(fk => fk.VinculoBNB).WithMany().HasForeignKey("VinculoBNB_Id");
            builder.HasOne(fk => fk.AreaDeNegocio).WithMany().HasForeignKey("AreaDeNegocio_Id");
        }
    }
}
