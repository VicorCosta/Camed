using Camed.SSC.Application.Requests.Inbox.Validators.Excluir;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Inbox.Commands.Excluir
{
    public class ExcluirInboxCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ExcluirInboxValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
