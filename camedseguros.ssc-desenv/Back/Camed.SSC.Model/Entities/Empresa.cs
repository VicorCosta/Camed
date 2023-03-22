using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Empresa : EntityBase
    {
        public string Nome { get; set; }
        public List<EmpresaTipoDeSeguro> TiposDeSeguro { get; set; }
        public Empresa()
        {
           TiposDeSeguro = new List<EmpresaTipoDeSeguro>();
        }
    }
}
