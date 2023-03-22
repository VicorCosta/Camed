using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class GrupoAgencia : EntityBase
    {
        public string Nome { get; set; }
        public ICollection<UsuarioGrupoAgencia> Usuarios { get; set; }

        public GrupoAgencia()
        {
            Usuarios = new HashSet<UsuarioGrupoAgencia>();
        }
    }
}
