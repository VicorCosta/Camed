using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class AgendamentoDeLigacao : EntityBase
    {
        public DateTime DataAgendamento { get; set; }
        public string Motivo { get; set; }
        public DateTime? DataLigacao { get; set; }
        public int Solicitacao_Id { get; set; }
        public int? TipoRetornoLigacao_Id { get; set; }
        public Solicitacao Solicitacao { get; set; }
        public TipoRetornoLigacao TipoRetornoLigacao { get; set; }
    }
}
