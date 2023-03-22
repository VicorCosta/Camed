using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class VendaCompartilhada : EntityBase
    {
        public Usuario Produtor { get; set; }
        public decimal Percentual { get; set; }
        public Acompanhamento Acompanhamento { get; set; }
    }
}
