using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class TiposDeSeguroTipoDeProduto
    {
        [Key]
        public int TipoDeProduto_Id { get; set; }
        [Key]
        public int TipoDeSeguro_Id { get; set; }

        public TipoDeProduto TipoDeProduto { get; set; }
        public TipoDeSeguro TipoDeSeguro { get; set; }
    }
}
