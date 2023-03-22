using Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaSinistro;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.ConsultaSinistro
{
    public class ConsultaSinistroValidator : AbstractValidator<ConsultaSinistroCommand>
    {
        public ConsultaSinistroValidator()
        {
            ValidateConsultaSinistro();
        }
        private void ValidateConsultaSinistro()
        {
        }
    }
}
