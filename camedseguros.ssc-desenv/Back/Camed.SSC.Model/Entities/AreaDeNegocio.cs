using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class AreaDeNegocio : EntityBase
    {
        public string Nome { get; set; }
        public ICollection<UsuarioAreaDeNegocio> Usuarios { get; set; }

        public AreaDeNegocio()
        {
            Usuarios = new HashSet<UsuarioAreaDeNegocio>();
        }
    }
}
