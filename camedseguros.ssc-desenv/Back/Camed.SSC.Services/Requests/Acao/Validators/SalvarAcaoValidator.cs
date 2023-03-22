using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAcaoValidator : AbstractValidator<SalvarAcaoCommand>
    {
        public SalvarAcaoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório");
               
            RuleFor(r => r.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatorio")
                .NotNull().WithMessage("Descrição é obrigatorio");

        }
    }
}
