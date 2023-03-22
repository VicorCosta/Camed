using Camed.SSC.Application.Requests;
using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarSituacaoCommandValidator : AbstractValidator<SalvarSituacaoCommand>
    {
        public SalvarSituacaoCommandValidator()
        {
            ValidateNome();
            ValidateTempo();
            ValidateTipo();
        }

        protected void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .MaximumLength(200).WithMessage("Tamanho máximo de 'Nome' é de 200 caracteres");
        }

        protected void ValidateTipo()
        {
            RuleFor(r => r.Tipo)
                .NotEmpty().WithMessage("'Contagem do SLA' é obrigatório")
                .MaximumLength(2).WithMessage("Tamanho máximo de 'Contagem do SLA' é de 2 caracater");
        }

        protected void ValidateTempo()
        {
            RuleFor(r => r.TempoSLA)
                .GreaterThanOrEqualTo(0).WithMessage("'Tempo de SLA' não pode ser negativo");

        }
    }
}
