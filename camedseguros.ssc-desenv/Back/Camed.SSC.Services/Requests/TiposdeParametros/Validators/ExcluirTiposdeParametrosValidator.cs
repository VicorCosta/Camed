using Camed.SSC.Application.Requests.TiposdeParametros.Commands.Excluir;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.TiposdeParametros.Validators
{
    class ExcluirTiposdeParametrosValidator: AbstractValidator<ExcluirTiposdeParametrosCommand>
    {
        public ExcluirTiposdeParametrosValidator()
        {
            ValidatorId();
            // ValidatorRelacao();
        }
        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }
    }
}
