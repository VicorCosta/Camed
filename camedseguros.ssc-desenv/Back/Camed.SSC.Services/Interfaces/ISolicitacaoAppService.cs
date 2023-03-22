using Camed.SSC.Application.ViewModel;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Interfaces
{
    public interface ISolicitacaoAppService
    {
        void Save(Domain.Entities.Solicitacao entity, List<AnexoDeSolicitacaoViewModel> anexos);
        void ValidarSolicitacao(Domain.Entities.Solicitacao solicitacao);
        void SaveCopy(Solicitacao entity, string login = null);
        int ObterNumero(int idDaSolicitacao);
        bool EFimDeFluxo(Solicitacao solicitacao);
        int ObterDiasRenovacao(DateTime inicio, DateTime fim);
        bool ExisteRegistroSolicitacaoAtendente(int idSolicitacao, int idAtendente);
        int ObterTempoSLA(DateTime inicio, DateTime fim);
        Acompanhamento ObterUltimoAcompanhamento(Solicitacao solicitacao);
    }
}
