using Camed.SSC.Application.Requests.Inbox.Commands.SalvarArquivo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Inbox.Validators.SalvarArquivo
{
    public class FormatarArquivoInboxValidator : AbstractValidator<FormatarArquivoInboxCommand>
    {
        public FormatarArquivoInboxValidator()
        {
            ValidateArquivos();

        }

        private void ValidateArquivos()
        {
        }
    }
}
