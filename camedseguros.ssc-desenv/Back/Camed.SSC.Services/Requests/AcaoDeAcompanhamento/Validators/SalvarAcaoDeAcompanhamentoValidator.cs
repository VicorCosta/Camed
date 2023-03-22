using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAcaoDeAcompanhamentoValidator : AbstractValidator<SalvarAcaoDeAcompanhamentoCommand>
    {
        public SalvarAcaoDeAcompanhamentoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório");

        }
    }
}
