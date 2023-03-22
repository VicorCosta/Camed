using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class VendaCompartilhadaMapping : IEntityTypeConfiguration<VendaCompartilhada>
    {
        public void Configure(EntityTypeBuilder<VendaCompartilhada> builder)
        {
            builder.ToTable("VendaCompartilhada");
            builder.HasKey(p => p.Id);

            builder.HasOne(f => f.Produtor).WithMany().HasForeignKey("Produtor_Id");
        }
    }
}
