using Camed.SSC.Core.Entities;
using System;

namespace Camed.SSC.Domain.Entities
{
    public class Feriado : EntityBase
    {
        public DateTime Data { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public Cidade Municipio { get; set; }
        public int? Municipio_Id { get; set; }
        public string Descricao { get; set; }
    }
}
