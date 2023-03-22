using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFeriadoValidator : AbstractValidator<SalvarFeriadoCommand>
    {
        public SalvarFeriadoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Data)
                .NotNull().WithMessage("'Data' é obrigatório")
                .MaximumLength(11).WithMessage("Tamanho máximo de 'data' é de 8 caracteres");
            RuleFor(r=>r.Pais)
                .NotNull().WithMessage("'Pais' é obrigatório")
                .MaximumLength(8).WithMessage("Tamanho máximo de 'pais' é de 8 caracteres");
            RuleFor(r => r.Descricao)
                .NotNull().WithMessage("'Descricao' é obrigatório");
            RuleFor(r => r.Estado)
                .MaximumLength(2).WithMessage("Tamanho máximo do 'estado' é de 2 caracteres")
                .NotNull().WithMessage("'Estado' é obrigatório");
            RuleFor(r => r.Municipio)
                .NotNull().WithMessage("'Cidade' é obrigatório")
                .NotEmpty().WithMessage("'Cidade' é obrigatório");

        }
    }
}
