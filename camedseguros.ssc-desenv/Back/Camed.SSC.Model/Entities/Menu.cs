using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class Menu : EntityBase
    {
        public string Label { get; set; }
        public string Rota { get; set; }
        public string Icone { get; set; }
        public Menu Superior { get; set; }
        public string Ajudatexto { get; set; }
        public ICollection<Menu> Submenus { get; set; }
        public ICollection<GrupoMenu> Grupos { get; set; }
        public ICollection<MenuAcao> MenuAcao { get; set; }

        public Menu()
        {
            Grupos = new HashSet<GrupoMenu>();
            MenuAcao = new HashSet<MenuAcao>();

        }
    }
}
