using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class SequencialDeSolicitacao : EntityBase
    {
        public Usuario Operador { get; set; }
        public DateTime Data { get; set; }
    }
}
