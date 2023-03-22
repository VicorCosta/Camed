using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class SolicitacaoMovimentacaoRamo : EntityBase
    {
        public virtual Solicitacao Solicitacao { get; set; }
        public virtual Usuario Atendente { get; set; }
    }
}
