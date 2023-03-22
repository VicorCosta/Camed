using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeCategoria : EntityBase
    {
        public string Descricao { get; set; }

        public TipoDeProduto TipoDeProduto { get; set; }
    }
}
