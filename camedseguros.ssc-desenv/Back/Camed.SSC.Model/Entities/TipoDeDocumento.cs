using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeDocumento : EntityBase
    {
        public string Nome { get; set; }
        public bool? DocumentoObrigatorio { get; set; }
        public string DescricaoDoDocumentoAnexo { get; set; }
        public bool Excluido { get; set; }

        public ICollection<TipoDeDocumentoTipoDeProduto> TiposDeProduto { get; set; }
        public ICollection<TipoDeDocumentoTipoDeProdutoTipoMorte> TiposDeProdutoMorte { get; set; }

        public TipoDeDocumento()
        {
            TiposDeProduto = new HashSet<TipoDeDocumentoTipoDeProduto>();
            TiposDeProdutoMorte = new HashSet<TipoDeDocumentoTipoDeProdutoTipoMorte>();
        }
    }
}
