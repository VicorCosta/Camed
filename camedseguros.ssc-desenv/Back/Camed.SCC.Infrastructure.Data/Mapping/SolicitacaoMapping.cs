using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.Data.Mapping
{
    public class SolicitacaoMapping : IEntityTypeConfiguration<Solicitacao>
    {
        public void Configure(EntityTypeBuilder<Solicitacao> builder)
        {
            builder.ToTable("Solicitacao");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Numero).IsRequired();
            builder.Property(p => p.DataDeIngresso).IsRequired();
            builder.Property(p => p.DadosAdicionais).IsRequired();
            builder.Property(p => p.DataHoraSituacaoAtual).IsRequired();
            builder.Property(p => p.OperacaoDeFinanciamento);
            builder.Property(p => p.Origem);
            builder.Property(p => p.CodigoDoBem);
            builder.Property(p => p.NumeroFinanciamento);
            builder.Property(p => p.Nu_Proposta_Seguradora);
            builder.Property(p => p.Nu_Apolice_Anterior);
            builder.Property(p => p.Pc_comissao);
            builder.Property(p => p.Co_Corretagem);
            builder.Property(p => p.Pc_agenciamento);
            builder.Property(p => p.VL_IS);
            builder.Property(p => p.Sede_Envia_Doc_Fisico);
            builder.Property(p => p.Nu_Sol_Vistoria);
            builder.Property(p => p.Cadastrado_GS);
            builder.Property(p => p.Cd_estudo);
            builder.Property(p => p.estudo_origem);
            builder.Property(p => p.DataFimVigencia);
            builder.Property(p => p.QtdDiasSLARenovacao);
            builder.Property(p => p.TipoEndosso);
            builder.Property(p => p.VIP);
            builder.Property(p => p.OrcamentoPrevio);
            builder.Property(p => p.CROSSUP);
            builder.Property(p => p.Mercado);
            builder.Property(p => p.Rechaco);
            builder.Property(p => p.vlr_premiotot_anterior);
            builder.Property(p => p.perc_comissao_anterior);
            builder.Property(p => p.vlr_premiotot_atual);
            builder.Property(p => p.perc_comissao_atual);
            builder.Property(p => p.VistoriaNec);
            builder.Property(p => p.ObsVistoria);
            builder.Property(p => p.TipoComissaoRV);
            builder.Property(p => p.Aplicacao);
            builder.Property(p => p.Rastreador);
            builder.Property(p => p.DataVencimento1aParc);
            builder.Property(p => p.vlr_premiotot_prop);
            builder.Property(p => p.Atendente_Id);

            builder.HasOne(f => f.Atendente).WithMany().HasForeignKey("Atendente_Id");
            builder.HasOne(f => f.Operador).WithMany().HasForeignKey("Operador_Id");
            builder.HasOne(f => f.Solicitante).WithMany().HasForeignKey("Solicitante_Id");
            builder.HasOne(f => f.Agencia).WithMany().HasForeignKey("Agencia_Id");
            builder.HasOne(f => f.Produtor).WithMany().HasForeignKey("Produtor_Id");
            builder.HasOne(f => f.TipoDeProduto).WithMany().HasForeignKey("TipoDeProduto_Id");
            builder.HasOne(f => f.CanalDeDistribuicao).WithMany().HasForeignKey("CanalDeDistribuicao_Id");
            builder.HasOne(f => f.TipoDeSeguro).WithMany().HasForeignKey("TipoDeSeguro_Id");
            builder.HasOne(f => f.Segurado).WithMany().HasForeignKey("Segurado_Id");
            builder.HasOne(f => f.Segmento).WithMany().HasForeignKey("Segmento_Id");
            builder.HasOne(f => f.AreaDeNegocio).WithMany().HasForeignKey("AreaDeNegocio_Id");
            builder.HasOne(f => f.SituacaoAtual).WithMany().HasForeignKey("SituacaoAtual_Id");
            builder.HasOne(f => f.Seguradora).WithMany().HasForeignKey("Seguradora_Id");
            builder.HasOne(f => f.Ramo).WithMany().HasForeignKey("Ramo_Id");
            builder.HasOne(f => f.TipoSeguroGS).WithMany().HasForeignKey("TipoSeguroGS_Id");
            builder.HasOne(f => f.FL_Forma_Pagamento_1a).WithMany().HasForeignKey("FL_Forma_Pagamento_1a_Id");
            builder.HasOne(f => f.FL_Forma_Pagamento_Demais).WithMany().HasForeignKey("FL_Forma_Pagamento_Demais_Id");
            builder.HasOne(f => f.GrupoDeProducao).WithMany().HasForeignKey("GrupoDeProducao_Id");
            builder.HasOne(f => f.TipoDeCategoria).WithMany().HasForeignKey("TipoDeCategoria_Id");
            builder.HasOne(f => f.TipoDeCancelamento).WithMany().HasForeignKey("TipoDeCancelamento_Id");
            builder.HasOne(f => f.MotivoEndossoCancelamento).WithMany().HasForeignKey("MotivoEndossoCancelamento_Id");
            builder.HasOne(f => f.MotivoRecusa).WithMany().HasForeignKey("MotivoRecusa_Id");
            builder.HasOne(f => f.AgenciaConta).WithMany().HasForeignKey("AgenciaConta_Id");
            builder.HasOne(f => f.SeguradoraCotacao).WithMany().HasForeignKey("SeguradoraCotacao_Id");
            builder.HasOne(f => f.CotacaoSombrero).WithMany().HasForeignKey("CotacaoSombrero_Id");

            builder.HasMany(f => f.Anexos).WithOne(f => f.Solicitacao).HasForeignKey("Solicitacao_Id");
            builder.HasMany(f => f.Acompanhamentos).WithOne(f => f.Solicitacao).HasForeignKey("Solicitacao_Id");
            builder.HasMany(f => f.AgendamentosDeLigacao).WithOne(f => f.Solicitacao).HasForeignKey("Solicitacao_Id");
            builder.HasMany(f => f.CheckList).WithOne(f => f.Solicitacao).HasForeignKey("Solicitacao_Id");
            builder.HasMany(f => f.Indicacoes).WithOne(f => f.Solicitacao).HasForeignKey("Solicitacao_Id");
            builder.HasMany(f => f.Checkins).WithOne(f => f.Solicitacao).HasForeignKey("Solicitacao_Id");

            builder.HasMany(f => f.Avaliacoes).WithOne(o => o.Solicitacao).HasForeignKey("Solicitacao_Id");
        }
    }
}
