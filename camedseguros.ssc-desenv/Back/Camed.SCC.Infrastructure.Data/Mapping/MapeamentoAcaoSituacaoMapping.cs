using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class MapeamentoAcaoSituacaoMapping : IEntityTypeConfiguration<MapeamentoAcaoSituacao>
    {
        public void Configure(EntityTypeBuilder<MapeamentoAcaoSituacao> builder)
        {
            builder.ToTable("MapeamentoAcaoSituacao");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.OrdemFluxo);
            builder.Property(p => p.OrdemFluxo2);
            builder.Property(p => p.ParametrosSistema_Id);

            builder.Property(p => p.PermiteEnvioDeArquivo).IsRequired();
            builder.Property(p => p.ExigeEnvioDeArquivo).IsRequired();
            builder.Property(p => p.ExigeObservacao).IsRequired();
            builder.Property(p => p.EnviaEmailSolicitante).IsRequired();
            builder.Property(p => p.EnviaEmailAtendente).IsRequired();
            builder.Property(p => p.EnviaEmailAoSegurado).IsRequired();
            builder.Property(p => p.EnviaSMSAoSegurado).IsRequired();

            builder.HasOne(o => o.SituacaoAtual).WithMany().HasForeignKey("SituacaoAtual_Id").IsRequired();
            builder.HasOne(o => o.Acao).WithMany().HasForeignKey("Acao_Id").IsRequired();
            builder.HasOne(o => o.ProximaSituacao).WithMany().HasForeignKey("ProximaSituacao_Id").IsRequired();
            builder.HasOne(o => o.Grupo).WithMany().HasForeignKey("Grupo_Id").IsRequired();

            builder.HasOne(o => o.ParametrosSistema).WithMany().HasForeignKey("ParametrosSistema_Id");
            builder.HasOne(o => o.ParametroSistemaSMS).WithMany().HasForeignKey("ParametroSistemaSMS_Id");
        }
    }
}
