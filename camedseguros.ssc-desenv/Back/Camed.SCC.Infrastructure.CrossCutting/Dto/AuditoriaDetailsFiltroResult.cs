using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SCC.Infrastructure.CrossCutting.Dto
{
    public class AuditoriaDetailsFiltroResult
    {

        public string Id { get; set; }
        public string ColumnName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public int? AuditLogId { get; set; }
       // public DateTime dt_evento { get; set; }
    }
}
