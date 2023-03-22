using Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaApolice;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.ConsultaApolice
{
    public class ConsultaApoliceValidator : AbstractValidator<ConsultaApoliceCommand>
    {
        public ConsultaApoliceValidator()
        {
            ValidateConsultaApolice();
        }
        private void ValidateConsultaApolice()
        {
        }
    }
}
