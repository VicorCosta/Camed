using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class Auditoria : EntityBase
    {
        public Auditoria()
        {
            AuditLogDetail = new HashSet<AuditLogDetail>();
        }

        public string TpReg { get; set; }
        public string OriginalId { get; set; }
        public string UserName { get; set; }
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public string TableName { get; set; }
        public int? Chave { get; set; }
        public int? NumeroDaSolicitacao { get; set; }
        public string Message { get; set; }
        public string MachineName { get; set; }
        public string RequestUrl { get; set; }
        public string Details { get; set; }
        //public AuditoriaDetalhe AuditoriaDetalhe { get; set; }

        public ICollection<AuditLogDetail> AuditLogDetail { get; set; }
    }
}
