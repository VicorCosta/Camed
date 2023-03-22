using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Expediente : EntityBase
    {

        public int Dia { get; set; }

        public DateTime HoraInicialManha { get; set; }

        public DateTime HoraFinalManha { get; set; }

        public DateTime HoraInicialTarde { get; set; }

        public DateTime HoraFinalTarde { get; set; }
    }
}

