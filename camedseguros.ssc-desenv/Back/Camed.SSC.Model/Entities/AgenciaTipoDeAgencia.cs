using Camed.SSC.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class AgenciaTipoDeAgencia : EntityBase
    {

        public TipoDeAgencia TipoDeAgencia { get; set; }
        public Agencia Agencia { get; set; }
    }
}
