using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeCancelamento : EntityBase
    {
        public virtual GrupoAgencia GrupoAgencia { get; set; }
        public string Descricao { get; set; }
    }
}
