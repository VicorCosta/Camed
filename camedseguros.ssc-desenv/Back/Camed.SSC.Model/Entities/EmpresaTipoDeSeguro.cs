using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class EmpresaTipoDeSeguro
    {
        [Key]
        public int Empresa_id { get; set; }
        [Key]
        public int TipoDeSeguro_id { get; set; }
        public bool Permitido_Abrir { get; set; }

        public Empresa Empresa { get; set; }
        public TipoDeSeguro TipoDeSeguro { get; set; }
    }
}
