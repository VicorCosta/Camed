using Camed.SSC.Core.Entities;
using System;

namespace Camed.SSC.Domain.Entities
{
    public class MapeamentoAcaoSituacao : EntityBase
    {
        protected MapeamentoAcaoSituacao()
        {

        }

        public MapeamentoAcaoSituacao(  Situacao situacaoAtual, 
                                        AcaoDeAcompanhamento acao,
                                        Situacao proximaSituacao,
                                        Grupo grupo, 
                                        bool permiteEnvioDeArquivo, 
                                        bool exigeEnvioDeArquivo,
                                        bool enviaEmailSolicitante,
                                        bool enviaEmailAtendente,
                                        bool exigeObservacao,
                                        bool enviaEmailAoSegurado,
                                        bool enviaSMSAoSegurado )
        {
            DefinirSituacaoAtual(situacaoAtual);
            DefinirAcao(acao);
            DefinirProximaSituacao(proximaSituacao);
            DefinirGrupo(grupo);

            PermiteEnvioDeArquivo = permiteEnvioDeArquivo;
            ExigeEnvioDeArquivo = exigeEnvioDeArquivo;
            EnviaEmailSolicitante = EnviaEmailSolicitante;
            EnviaEmailAtendente = EnviaEmailAtendente;
            ExigeObservacao = exigeObservacao;
            EnviaEmailAoSegurado = enviaEmailAoSegurado;
            EnviaSMSAoSegurado = enviaSMSAoSegurado;
        }

        public void DefinirSituacaoAtual(Situacao situacaoAtual)
        {
            this.SituacaoAtual = situacaoAtual ?? throw new ApplicationException("'Situação atual' não informada");
        }

        public void DefinirAcao(AcaoDeAcompanhamento acao)
        {
            this.Acao = acao ?? throw new ApplicationException("'Ação de acompanhamento' não informada");
        }

        public void DefinirProximaSituacao(Situacao proximaSituacao)
        {
            this.ProximaSituacao = proximaSituacao ?? throw new ApplicationException("'Próxima situação' não informada");
        }

        public void DefinirGrupo(Grupo grupo)
        {
            this.Grupo = grupo ?? throw new ApplicationException("'Grupo' não informado");
        }


        public int? OrdemFluxo { get; set; }
        public int? OrdemFluxo2 { get; set; }
        public int? ParametrosSistema_Id { get; set; }
        public int SituacaoAtual_Id { get; set; }
        public int Acao_Id { get; set; }
        public Situacao SituacaoAtual { get; set; }
        public AcaoDeAcompanhamento Acao { get; set; }
        public int ProximaSituacao_Id { get; set; }
        public Situacao ProximaSituacao { get; set; }
        public Grupo Grupo { get; set; }
        public bool PermiteEnvioDeArquivo { get; set; }
        public bool ExigeEnvioDeArquivo { get; set; }
        public bool EnviaEmailSolicitante { get; set; }
        public bool EnviaEmailAtendente { get; set; }
        public bool ExigeObservacao { get; set; }
        public bool EnviaEmailAoSegurado { get; set; }
        public ParametrosSistema ParametrosSistema { get; set; }
        public bool EnviaSMSAoSegurado { get; set; }
        public ParametrosSistema ParametroSistemaSMS { get; set; }
    }
}
