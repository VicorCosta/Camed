using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class SolCheckList : EntityBase
    {
        public virtual Solicitacao Solicitacao { get; set; }
        public virtual TipoDeDocumento TipoDeDocumento { get; set; }
        public bool DocumentoAnexado { get; set; }
        public bool DocumentoAnexadoConfirmado { get; set; }
    }
}
