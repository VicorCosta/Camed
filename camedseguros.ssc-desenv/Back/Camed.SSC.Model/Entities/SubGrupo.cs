using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class SubGrupo
    {
        [Key]
        public int Grupo_Id { get; set; }

        [Key]
        public int Subgrupo_Id { get; set; }

        [ForeignKey("Grupo_Id")]
        public Grupo Grupo { get; set; }

        [ForeignKey("Subgrupo_Id")]
        public Grupo Subgrupo { get; set; }
    }
}
