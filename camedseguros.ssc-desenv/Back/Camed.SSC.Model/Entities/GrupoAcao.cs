using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class GrupoAcao
    {
        [Key]
        public virtual int Grupo_Id { get; set; }
        [Key]
        public virtual int Acao_Id { get; set; }
        public Grupo Grupo { get; set; }
        public Acao Acao { get; set; }
    }
}
