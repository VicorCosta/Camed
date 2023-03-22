using Camed.SSC.Application.Requests.Solicitacao.Validators.SalvarArquivoBNB;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivoBNB
{
    public class SalvarArquivoBNBCommand : CommandBase, IRequest<IResult>
    {
        public List<AnexoBNBViewModel> anexos;

        public override bool IsValid()
        {
            ValidationResult = new SalvarArquivoBNBValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
