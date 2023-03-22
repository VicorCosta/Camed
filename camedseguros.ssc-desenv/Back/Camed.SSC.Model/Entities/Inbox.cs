using Camed.SSC.Core.Entities;
using System;
using System.Collections.Generic;

namespace Camed.SSC.Domain.Entities
{
    public class Inbox : EntityBase
    {
        public Usuario UsuarioRemetente { get; set; }
        public Usuario UsuarioDestinatario { get; set; }

        public string Texto { get; set; }
        public string Assunto { get; set; }
        public ICollection<AnexosDeInbox> Anexos { get; set; }

        public bool Lida { get; set; }
        public DateTime? DataLida { get; set; }

        public bool VisivelEntrada { get; set; }
        public bool VisivelSaida { get; set; }

        public DateTime DataCriacao { get; set; }
        public int? Solicitacao_Id { get; set; }
        public virtual Solicitacao numeroSolicitacao { get; set; }

        public int MensagemResp_Id { get; set; }
    }
}