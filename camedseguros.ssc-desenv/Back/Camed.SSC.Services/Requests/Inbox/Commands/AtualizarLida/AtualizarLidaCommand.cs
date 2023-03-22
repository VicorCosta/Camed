using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Inbox.Commands.MarcarMensagemComoLida
{
    public class AtualizarLidaCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public bool Lida { get; set; }
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
