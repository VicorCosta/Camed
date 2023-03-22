using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Segurado : EntityBase
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string EmailSecundario { get; set; }
        public string TelefonePrincipal { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneAdicional { get; set; }
        public string Contato { get; set; }
        public VinculoBNB VinculoBNB { get; set; }

    }
}
