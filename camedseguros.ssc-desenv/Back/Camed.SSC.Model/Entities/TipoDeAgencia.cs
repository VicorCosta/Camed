using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeAgencia : EntityBase
    {
        public string Nome { get; set; }
        public GrupoAgencia GrupoAgencia { get; set; }

        public ICollection<AgenciaTipoDeAgencia> Agencias { get; set; }

        public TipoDeAgencia()
        {
            Agencias = new HashSet<AgenciaTipoDeAgencia>();
        }
    }
}
