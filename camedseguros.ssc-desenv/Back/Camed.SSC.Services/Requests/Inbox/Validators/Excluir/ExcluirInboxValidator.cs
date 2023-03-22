using Camed.SSC.Application.Requests.Inbox.Commands.Excluir;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Inbox.Validators.Excluir
{
    public class ExcluirInboxValidator : AbstractValidator<ExcluirInboxCommand>
    {
        public ExcluirInboxValidator()
        {
            ValidateExclusao();

        }

        private void ValidateExclusao()
        {}
    }
}
