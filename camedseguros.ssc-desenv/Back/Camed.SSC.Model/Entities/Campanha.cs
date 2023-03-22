using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Campanha : EntityBase
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
    }
}