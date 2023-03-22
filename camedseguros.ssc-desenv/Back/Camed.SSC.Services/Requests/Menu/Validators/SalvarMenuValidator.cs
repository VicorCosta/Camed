using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMenuValidator : AbstractValidator<SalvarMenuCommand>
    {
        public SalvarMenuValidator()
        {
            ValidateLabel();
            ValidateIcone();
            ValidateRota();
        }

        private void ValidateLabel()
        {
            RuleFor(r => r.Label).NotNull().WithMessage("'Nome' é obrigatório");

            RuleFor(r => r.Label)
                .NotEmpty().WithMessage("'Nome' formato inválido").When(r => r.Label != null);
        }

        private void ValidateIcone()
        {
            RuleFor(r => r.Icone).Null().When(w => w.MenuSuperior.HasValue == true).WithMessage("'Icone' é obrigatorio");

            RuleFor(r => r.Icone).NotNull().When(w => w.MenuSuperior.HasValue == false).WithMessage("'Icone' é obrigatorio");
        }

        private void ValidateRota() {
            RuleFor(r => r.Rota).NotNull().WithMessage("'Rota' é obrigatório").When(r => r.MenuSuperior != null);

            RuleFor(r => r.Rota)
                .NotEmpty().WithMessage("'Rota' formato inválido").When(r => r.MenuSuperior != null && r.Rota == " ");
        }


    }
}
