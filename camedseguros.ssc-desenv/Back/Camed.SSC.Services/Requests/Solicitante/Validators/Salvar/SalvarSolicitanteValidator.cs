using Camed.SSC.Application.Requests.Solicitante.Command.Excluir;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitante.Validators.Salvar
{
    public class SalvarSolicitanteValidator : AbstractValidator<SalvarSolicitanteCommand>
    {
        public SalvarSolicitanteValidator()
        {
        }
    }
}
