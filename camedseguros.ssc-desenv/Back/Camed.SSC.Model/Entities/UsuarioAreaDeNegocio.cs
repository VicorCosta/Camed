using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class UsuarioAreaDeNegocio
    {
        [Key]
        public int Usuario_Id { get; set; }
        [Key]
        public int AreaDeNegocio_Id { get; set; }


        public Usuario Usuario { get; set; }
        public AreaDeNegocio AreaDeNegocio { get; set; }
    }
}
