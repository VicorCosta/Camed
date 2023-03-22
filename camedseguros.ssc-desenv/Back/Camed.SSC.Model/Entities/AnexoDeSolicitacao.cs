using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class AnexoDeSolicitacao : EntityBase
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public virtual Solicitacao Solicitacao { get; set; }
    }
}
