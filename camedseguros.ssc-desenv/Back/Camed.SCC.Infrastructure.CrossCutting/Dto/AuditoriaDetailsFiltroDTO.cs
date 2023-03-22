using System;

namespace Camed.SCC.Infrastructure.CrossCutting.Dto
{
    public class AuditoriaDetailsFiltroDTO
    {
        public string RecordId { get; set; }
        //public int? Id { get; set; }
        //public string ColumnName { get; set; }
        //public string OriginalValue { get; set; }
        //public string NewValue { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}
