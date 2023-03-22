using Camed.SSC.Core.Entities;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class MapeamentoAtendenteCopy : EntityBase
    {
        public ICollection<TipoDeSeguro> TipoDeSeguro  { get; set; }
        public ICollection<GrupoAgencia> GrupoAgencia { get; set; }
        public ICollection<Agencia> Agencia { get; set; }
        public ICollection<AreaDeNegocio> AreaDeNegocio { get; set; }
        public ICollection<Usuario> Atendente { get; set; }


        public MapeamentoAtendenteCopy()
        {
            TipoDeSeguro = new HashSet<TipoDeSeguro>();
            GrupoAgencia = new HashSet<GrupoAgencia>();
            Agencia = new HashSet<Agencia>();
            AreaDeNegocio = new HashSet<AreaDeNegocio>();
            Atendente = new HashSet<Usuario>();
        }
    }
}
