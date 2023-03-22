using Camed.SSC.Application.Requests;
using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirSituacaoCommandValidator : AbstractValidator<ExcluirSituacaoCommand>
    {
        public ExcluirSituacaoCommandValidator()
        {
            ValidateId();
        }

        void ValidateId()
        {
            RuleFor(r => r.Id)
                   .GreaterThan(0).WithMessage("'Id' deve ser maior que zero");
        }
    }
}
