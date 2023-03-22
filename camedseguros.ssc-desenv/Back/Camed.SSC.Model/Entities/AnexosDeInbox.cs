using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class AnexosDeInbox : EntityBase
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public int Inbox_Id { get; set; }
        public virtual Inbox Inbox { get; set; }
    }
}
