using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMapeamentoDeAtendenteValidator : AbstractValidator<SalvarMapeamentoDeAtendenteCommand>
    {
        public SalvarMapeamentoDeAtendenteValidator()
        {
            ValidateAgencia();
            ValidateTipoSeguro();
            ValidateAreaNegocio();
            ValidateGrupoAgencia();
            ValidateAtendente();
        }

        private void ValidateAgencia()
        {

            RuleFor(r => r.Agencia_Id)
                .NotNull().WithMessage("'Agência' é obrigatória");
        }

        private void ValidateTipoSeguro()
        {
            RuleFor(r => r.TipoDeSeguro_Id)
                .NotNull().WithMessage("'Tipo de Seguro' é obrigatório");
        }
        private void ValidateAreaNegocio()
        {
            RuleFor(r => r.AreaDeNegocio_Id)
                .NotNull().WithMessage("'Área de Negócio' é obrigatório");
        }
        private void ValidateGrupoAgencia()
        {
            RuleFor(r => r.GrupoAgencia_Id)
                .NotNull().WithMessage("'Ramo de Negócio' é obrigatória");
        }
        private void ValidateAtendente()
        {
            RuleFor(r => r.Atendente_Id)
                .NotNull().WithMessage("'Atendente' é obrigatória");
        }
    }
}
