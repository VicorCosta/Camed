using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class AuditLogDetail : EntityBase
    {
        public string ColumnName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public int AuditLogId { get; set; }
        public Auditoria Auditoria { get; set; }
    }
}
