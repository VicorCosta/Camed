using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class Grupo : EntityBase
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool SempreVisualizarObservacao { get; set; }
        public bool AtribuirAtendente { get; set; }
        public bool AtribuirOperador { get; set; }
        public bool CancelarSolicitacao { get; set; }

        public ICollection<GrupoMenu> Menus { get; set; }
        public ICollection<SubGrupo> SubGrupos { get; set; }
        public ICollection<GrupoAcao> Acoes { get; set; }
        
        public Grupo()
        {
            Acoes = new HashSet<GrupoAcao>();
            Menus = new HashSet<GrupoMenu>();
            SubGrupos = new HashSet<SubGrupo>();
        }

    }
}
