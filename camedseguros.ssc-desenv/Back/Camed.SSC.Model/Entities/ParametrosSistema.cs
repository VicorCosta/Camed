using Camed.SSC.Core.Entities;

namespace Camed.SSC.Domain.Entities
{
    public class ParametrosSistema : EntityBase
    {
        public string Parametro { get; set; }
        public string Valor { get; set; } 
        public string Tipo { get; set; }
        public int? TipoDeParametro_Id { get; set; }
        public int? VariaveisDeEmail_Id { get; set; }

        public virtual VariaveisDeEmail VariaveisDeEmail { get; set; }
        public virtual TiposdeParametros TipoDeParametro { get; set; }

    }
}
