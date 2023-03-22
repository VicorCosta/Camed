using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class TipoDeSeguro : EntityBase
    {
        public string Nome { get; set; }
        public GrupoAgencia GrupoAgencia { get; set; }
        public virtual ICollection<TipoDeSeguroTipoDeProduto> TiposDeProduto { get; set; }
        public ICollection<EmpresaTipoDeSeguro> Empresas { get; set; }

        public TipoDeSeguro()
        {
            TiposDeProduto = new HashSet<TipoDeSeguroTipoDeProduto>();
            Empresas = new HashSet<EmpresaTipoDeSeguro>();
        }
    }
}
