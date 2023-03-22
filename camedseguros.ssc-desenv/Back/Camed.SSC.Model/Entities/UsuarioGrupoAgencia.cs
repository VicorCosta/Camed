using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class UsuarioGrupoAgencia
    {
        [Key]
        public int Usuario_Id { get; set; }
        [Key]
        public int GrupoAgencia_Id { get; set; }


        public Usuario Usuario { get; set; }
        public GrupoAgencia GrupoAgencia { get; set; }
    }
}
