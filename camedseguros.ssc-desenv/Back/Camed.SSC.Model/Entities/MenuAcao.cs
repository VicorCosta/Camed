using Camed.SSC.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class MenuAcao
    {
        [Key]
        public int id_menu_acao { get; set; }
        public int menu_id { get; set; }
        public int acao_id { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("acao_id")]
        public Acao Acao{ get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("menu_id")]
        public Menu Menu { get; set; }
    }
}
