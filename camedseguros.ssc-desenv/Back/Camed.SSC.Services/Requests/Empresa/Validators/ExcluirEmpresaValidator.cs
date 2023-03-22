using System;
using System.Linq;
using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirEmpresaValidator : AbstractValidator<ExcluirEmpresaCommand>
    {
        public ExcluirEmpresaValidator()
        {
            ValidatorId();
           // ValidatorRelacao();
        }

        private void ValidatorRelacao()
        {
            RuleFor(r => r.TiposDeSeguro).NotNull().NotEmpty().WithMessage("Contem uma relação tipo de seguro");
        }

        private void ValidatorId()
        {
            RuleFor(r => r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero ");
        }

    }
}
