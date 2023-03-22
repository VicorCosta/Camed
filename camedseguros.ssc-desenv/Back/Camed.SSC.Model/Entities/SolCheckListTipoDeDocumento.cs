using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Domain.Entities
{
    public class SolCheckListTipoDeDocumento
    {
        [Key]
        public int TipoDeDocumento_Id { get; set; }
        [Key]
        public int SolCheckList_Id { get; set; }

        public TipoDeDocumento TipoDeDocumento { get; set; }
        public SolCheckList SolCheckList { get; set; }
    }
}