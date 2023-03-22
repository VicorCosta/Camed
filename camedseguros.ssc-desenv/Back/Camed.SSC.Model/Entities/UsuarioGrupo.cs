using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class UsuarioGrupo : Camed.SSC.Core.Entities.EntityBase
    {
        [Key]
        public int Usuario_Id { get; set; }
        [Key]
        public int Grupo_Id { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public Grupo Grupo { get; set; }
    }
}
