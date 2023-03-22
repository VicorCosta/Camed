using Camed.SSC.Core.Entities;

namespace Camed.SSC.Domain.Entities
{
    public class Situacao : EntityBase
    {
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public int? TempoSLA { get; set; }
        public bool EFimFluxo { get; set; }
        public bool PendenciaCliente { get; set; }
    }
}
