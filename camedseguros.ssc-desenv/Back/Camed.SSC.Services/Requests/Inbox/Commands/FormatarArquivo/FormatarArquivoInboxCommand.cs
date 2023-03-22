using Camed.SSC.Application.Requests.Inbox.Validators.SalvarArquivo;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Inbox.Commands.SalvarArquivo
{
    public class FormatarArquivoInboxCommand : CommandBase, IRequest<IResult>
    {
        public List<IFormFile> FormFiles;
        public override bool IsValid()
        {
            ValidationResult = new FormatarArquivoInboxValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
