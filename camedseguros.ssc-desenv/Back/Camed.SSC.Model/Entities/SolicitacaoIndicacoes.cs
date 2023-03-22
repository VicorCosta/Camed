using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Camed.SSC.Domain.Entities
{
    public class SolicitacaoIndicacoes : EntityBase
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public Solicitacao Solicitacao { get; set; }
    }
}
