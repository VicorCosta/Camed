using Camed.SSC.Application.Requests.Solicitacao.Validators.ConsultaApolice;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaApolice
{
    public class ConsultaApoliceCommand : CommandBase, IRequest<IResult>
    {
        public string cpf_cnpj { get; set; }
        public string apolice { get; set; }
        public string tipo { get; set; }
        public string Link { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ConsultaApoliceValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
