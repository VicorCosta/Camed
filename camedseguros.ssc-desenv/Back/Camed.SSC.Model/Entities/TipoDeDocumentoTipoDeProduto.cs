using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeDocumentoTipoDeProduto
    {
        [Key]
        public int TipoDeDocumento_Id { get; set; }
        [Key]
        public int TipoDeProduto_Id { get; set; }

        public TipoDeDocumento TipoDeDocumento { get; set; }
        public TipoDeProduto TipoDeProduto { get; set; }
    }
}
