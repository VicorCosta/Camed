using Camed.SCC.Infrastructure.CrossCutting.Dto;
using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Camed.SCC.Infrastructure.Data.Context
{
    public class SSCContext : DbContext
    {
        public DbSet<AvAtendimento> AvAtendimento { get; set; }
        public DbSet<Situacao> Situacoes { get; set; }
        public DbSet<AcaoDeAcompanhamento> AcoesDeAcompanhamento { get; set;}
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<VinculoBNB> VinculosBNB { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<CanalDeDistribuicao> CanaisDeDistribuicao { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<MotivoRecusa> MotivosRecusa { get; set; }
        public DbSet<TipoRetornoLigacao> TiposRetornoLigacao { get; set; }
        public DbSet<GrupoAgencia> GruposAgencia { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<TipoDeSeguro> TiposDeSeguro { get; set; }
        public DbSet<TipoDeCategoria> TiposdeCategoria { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Acao> Acoes { get; set; }
        public DbSet<MenuAcao> MenuAcoes { get; set; }
        public DbSet<AreaDeNegocio> AreasDeNegocio { get; set; }
        public DbSet<TipoDeDocumento> TiposDeDocumento { get; set; }
        public DbSet<TipoDeAgencia> TiposDeAgencia { get; set; }
        public DbSet<TipoDeCancelamento> TiposDeCancelamento { get; set; }
        public DbSet<AgendamentoDeLigacao> AgendamentosDeLigacao { get; set; }
        public DbSet<AgendamentoDeLigacaoGeral> AgendamentosDeLigacaoGeral { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<AuditLogDetail> AuditLogDetail { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<TipoDeProduto> TiposDeProduto { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<MapeamentoAcaoSituacao> MapeamentosAcaoSituacao { get; set; }
        public DbSet<ParametrosSistema> ParametrosSistema { get; set; }
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<AnexosDeInbox> AnexosDeInbox { get; set; }
        public DbSet<Papel> Papeis { get; set; }
        public DbSet<Agencia> Agencias { get; set; }
        public DbSet<AnexoDeSolicitacao> AnexosDeSolicitacao { get; set; }
        public DbSet<AnexoDeAcompanhamento> AnexosDeAcompanhamento { get; set; }
        public DbSet<VendaCompartilhada> VendasCompartilhadas { get; set; }
        public DbSet<Acompanhamento> Acompanhamentos { get; set; }
        public DbSet<SolCheckList> SolCheckLists { get; set; }
        public DbSet<SolicitacaoIndicacoes> SolicitacaoIndicacoes { get; set; }
        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<Solicitante> Solicitantes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Segmento> Segmentos { get; set; }
        public DbSet<VW_SEGURADORA> Seguradoras { get; set; }
        public DbSet<Ramo> Ramos { get; set; }
        public DbSet<TipoDeSeguroGS> TiposDeSeguroGS { get; set; }
        public DbSet<FormaDePagamento> FormasDePagamento { get; set; }
        public DbSet<GrupoDeProducao> GruposDeProducao { get; set; }
        public DbSet<MotivoEndossoCancelamento> MotivosEndossoCancelamento { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<Segurado> Segurados { get; set; }
        public DbSet<MapeamentoAreaDeNegocio> MapeamentosAreaDeNegocio { get; set; }
        public DbSet<MapeamentoAtendente> MapeamentosAtendente { get; set; }
        public DbSet<SequencialDeSolicitacao> SequencialDeSolicitacoes { get; set; }
        public DbSet<SolicitacaoMovimentacaoRamo> SolicitacaoMovimentacoesRamos { get; set; }
        public DbSet<TipoMorte> TiposMorte { get; set; }
        public DbSet<TextoParametrosSistema> TextosParametrosSistemas { get; set; }
        public DbSet<TextoParametroSeguradora> TextosParametrosSeguradoras { get; set; }
        public DbSet<SLAAgendamento> SLAAgendamentos { get; set; }
        public DbSet<VariaveisDeEmail> VariaveisDeEmails { get; set; }
        public DbSet<AlteracaoSenhaUsuario> AlteracaoSenhaUsuarios { get; set; }
        public SSCContext(DbContextOptions<SSCContext> options) : base(options)
      
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Query<ApoliceGsDTO>();
            modelBuilder.Query<AuditoriaFiltroResult>();
            modelBuilder.Query<ResultProcedure>();
            modelBuilder.Query<AuditoriaDetailsFiltroResult>();

            var ApplyConfigurationMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                           .Single(m => m.Name == nameof(ModelBuilder.ApplyConfiguration) &&
                                                        m.GetParameters().Count() == 1 &&
                                                        m.GetParameters().Single().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            var entities = Assembly.GetExecutingAssembly().GetTypes()
                .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters);

            foreach (var type in entities)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = ApplyConfigurationMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                    }
                }
            }
        }
    }
}
