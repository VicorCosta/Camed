using Camed.SSC.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities {
  public class TipoDeDocumentoTipoDeProdutoTipoMorte : EntityBase {

        [Key]
        public new int Id { get; set; }
        [Key]
        public int TipoDeDocumento_Id { get; set; }
        [Key]
        public int? TipoDeProduto_Id { get; set; }
        [Key]
        public int? TipoMorte_Id { get; set; }
        public bool Obrigatorio { get; set; }
        public int? Ordem { get; set; }
        public bool Ativo { get; set; } // Não existe essa coluna no DB

        public TipoDeDocumento TipoDeDocumento { get; set; }
        public TipoDeProduto TipoDeProduto { get; set; }
        public TipoMorte TipoMorte { get; set; }

    }
}