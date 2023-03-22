using Camed.SSC.Core.Entities;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class AusenciaAtendente : EntityBase
    {
        public DateTime DataInicioAusencia { get; set; }
        public DateTime DataFinalAusencia  { get; set; }
        public int Atendente_Id { get; set; }   
        public virtual Usuario Atendente { get; set; }
    }
}