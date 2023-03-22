using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Papel : EntityBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Acao> Acoes { get; set; }
        public Papel()
        {
            Acoes = new HashSet<Acao>();
        }
    }
}
