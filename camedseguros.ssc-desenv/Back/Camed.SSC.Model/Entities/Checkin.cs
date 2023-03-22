using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Checkin : EntityBase 
    {
        public Checkin()
        {
            this.DataHora = DateTime.Now;
        }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Localidade { get; set; }
        public string Endereco { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Solicitacao Solicitacao { get; set; }
        public DateTime DataHora { get; set; }
    }
}
