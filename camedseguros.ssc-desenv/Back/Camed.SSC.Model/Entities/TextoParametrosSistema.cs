using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TextoParametrosSistema : EntityBase
    {
        public int TipoDeSeguro_Id { get; set; }
        public int TipoDeProduto_Id { get; set; }
        public string Texto { get; set; }

        public virtual TipoDeSeguro TipoDeSeguro { get; set; }
        public virtual TipoDeProduto TipoDeProduto { get; set; }
    }
}
