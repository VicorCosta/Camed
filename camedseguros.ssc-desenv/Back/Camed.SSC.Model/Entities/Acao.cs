using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class Acao : EntityBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Papel Papel { get; set; }
        public virtual ICollection<GrupoAcao> Grupos { get; set; }
        public ICollection<MenuAcao> MenuAcaos{ get; set; }

        public Acao()
        {
            this.Grupos = new HashSet<GrupoAcao>();
        }
    }
}
