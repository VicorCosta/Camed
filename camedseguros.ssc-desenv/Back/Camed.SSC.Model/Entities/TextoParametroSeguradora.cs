using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TextoParametroSeguradora : EntityBase
    {
        public int Seguradora_Id { get; set; }
        public virtual VW_SEGURADORA Seguradora { get; set; }
        public string Texto { get; set; }
    }
}
