using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class GrupoMenu
    {
        [Key]
        public int Grupo_Id { get; set; }
        [Key]
        public int Menu_Id { get; set; }

        public Grupo Grupo { get; set; }
        public Menu Menu { get; set; }
    }
}
