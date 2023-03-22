using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class Frame : EntityBase
    {
        public Frame()
        {
            AcoesAcompanhamento = new HashSet<FrameAcaoDeAcompanhamento>();
        }

        public string Nome { get; set; }
        public ICollection<FrameAcaoDeAcompanhamento> AcoesAcompanhamento { get; set; }
    }
}
