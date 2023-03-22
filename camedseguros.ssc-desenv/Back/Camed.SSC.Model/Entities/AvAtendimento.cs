using System;
using System.Collections.Generic;
using System.Text;
using Camed.SSC.Core.Entities;

namespace Camed.SSC.Domain.Entities
{
    public class AvAtendimento: EntityBase
    {
        public string Observacao {get ; set;}
        public string Nota { get; set; }
        public int Solicitacao_Id { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int Usuario_Id { get; set; }

        public virtual Solicitacao Solicitacao { get; set; }

    }
}
