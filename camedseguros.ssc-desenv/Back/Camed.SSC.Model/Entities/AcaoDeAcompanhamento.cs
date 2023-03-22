using Camed.SSC.Core.Entities;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class AcaoDeAcompanhamento : EntityBase
    {
        public string Nome { get; set; }
        public  ICollection<FrameAcaoDeAcompanhamento> Frames { get; set; }

        public AcaoDeAcompanhamento()
        {
            Frames = new HashSet<FrameAcaoDeAcompanhamento>();
        }
    }
}
