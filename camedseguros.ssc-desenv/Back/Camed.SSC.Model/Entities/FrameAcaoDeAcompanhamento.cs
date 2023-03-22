using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class FrameAcaoDeAcompanhamento
    {
        [Key]
        public int AcaoDeAcompanhamento_Id { get; set; }
        [Key]
        public int Frame_Id { get; set; }

        public AcaoDeAcompanhamento AcaoDeAcompanhamento { get; set; }
        public Frame Frame { get; set; }
    }
}
