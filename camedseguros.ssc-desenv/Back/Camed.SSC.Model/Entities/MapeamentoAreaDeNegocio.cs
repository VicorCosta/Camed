using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class MapeamentoAreaDeNegocio : EntityBase {
        public virtual TipoDeAgencia TipoDeAgencia { get; set; }
        public virtual TipoDeSeguro TipoDeSeguro { get; set; }
        public int? OperacaoDeFinanciamento { get; set; }
        public virtual TipoDeProduto TipoDeProduto { get; set; }
        public virtual VinculoBNB VinculoBNB { get; set; }
        public virtual AreaDeNegocio AreaDeNegocio { get; set; }


    }
}
