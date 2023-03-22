using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Agencia : EntityBase
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int SuperId { get; set; }
        public string Super { get; set; }

        public ICollection<AgenciaTipoDeAgencia> TiposDeAgencias { get; set; }

        public Agencia()
        {
            TiposDeAgencias = new HashSet<AgenciaTipoDeAgencia>();
        }
    }
}
