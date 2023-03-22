using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Ramo : EntityBase
    {
        public string Nm_Ramo { get; set; }
        public int? Seguradora_Id { get; set; }
        public VW_SEGURADORA Seguradora { get; set; }
    }
}
