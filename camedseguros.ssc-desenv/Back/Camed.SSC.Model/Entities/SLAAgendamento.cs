using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class SLAAgendamento : EntityBase
    {
        public string Situacao { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DataEHoraInicial { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataEHoraFinal { get; set; }
    }
}
