using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeProduto : EntityBase
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int Situacao_Id { get; set; }
        public Situacao Situacao { get; set; }
        public int SlaMaximo { get; set; }
        public bool UsoInterno { get; set; }
        public string DescricaoSasParaTipoDeProduto { get; set; }
        public string ObservacaoTipoDeProduto { get; set; }
        public Situacao SituacaoRenovacao { get; set; }


        public ICollection<TipoDeSeguroTipoDeProduto> TiposDeSeguro { get; set; }
        public ICollection<TipoDeDocumentoTipoDeProduto> TiposDeDocumento { get; set; }
        public ICollection<TipoDeDocumentoTipoDeProdutoTipoMorte> TiposDeProdutoMorte { get; set; }

    }
}
