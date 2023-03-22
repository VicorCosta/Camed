using Camed.SSC.Core.Entities;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class MapeamentoAtendente : EntityBase
    {
        public int TipoDeSeguro_Id { get; set; }
        public int GrupoAgencia_Id { get; set; }
        public int Agencia_Id { get; set; }
        public int AreaDeNegocio_Id { get; set; }
        public int Atendente_Id { get; set; }


        public virtual TipoDeSeguro TipoDeSeguro { get; set; }
        public virtual GrupoAgencia GrupoAgencia { get; set; }
        public virtual Agencia Agencia { get; set; }
        public virtual AreaDeNegocio AreaDeNegocio { get; set; }
        public virtual Usuario Atendente { get; set; }
    }
}
