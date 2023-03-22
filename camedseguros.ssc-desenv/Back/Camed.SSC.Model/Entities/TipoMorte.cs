using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TipoMorte : EntityBase
    {
        public string Descricao { get; set; }

        public ICollection<TipoDeDocumentoTipoDeProdutoTipoMorte> TiposDeProdutoMorte { get; set; }
    }
}
