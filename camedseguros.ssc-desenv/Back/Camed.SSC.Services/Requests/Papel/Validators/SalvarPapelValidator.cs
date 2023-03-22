using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarPapelValidator : AbstractValidator<SalvarPapelCommand>
    {
        public SalvarPapelValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório");
               
            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório");

        }
    }
}
