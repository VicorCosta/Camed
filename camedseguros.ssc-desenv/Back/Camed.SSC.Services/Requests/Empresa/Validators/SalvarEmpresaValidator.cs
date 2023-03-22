using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarEmpresaValidator : AbstractValidator<SalvarEmpresaCommand>
    {
        public SalvarEmpresaValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório")
                .MaximumLength(200).WithMessage("Tamanho máximo de 'Empresa' é de 200 caracteres");
        }
    }
}
