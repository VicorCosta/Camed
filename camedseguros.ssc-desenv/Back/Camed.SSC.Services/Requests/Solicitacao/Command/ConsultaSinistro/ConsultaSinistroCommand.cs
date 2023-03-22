using Camed.SSC.Application.Requests.Solicitacao.Validators.ConsultaSinistro;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaSinistro
{
    public class ConsultaSinistroCommand : CommandBase, IRequest<IResult>
    {
        public string cpf_cnpj { get; set; } 
        public string apolice { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ConsultaSinistroValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
