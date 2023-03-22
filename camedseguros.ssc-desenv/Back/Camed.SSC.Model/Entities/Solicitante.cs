using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Solicitante : EntityBase
    {
        public virtual Usuario Usuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string TelefonePrincipal { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneAdicional { get; set; }
    }
}
