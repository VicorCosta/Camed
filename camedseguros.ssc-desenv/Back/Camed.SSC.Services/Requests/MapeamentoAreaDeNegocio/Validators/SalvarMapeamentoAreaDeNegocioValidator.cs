using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMapeamentoAreaDeNegocioValidator : AbstractValidator<SalvarMapeamentoAreaDeNegocioCommand>
    {
        public SalvarMapeamentoAreaDeNegocioValidator()
        {

            ValidateTipoAgencia();
            ValidateTipoDeSeguro();
            ValidateRamoDeSeguro();
            ValidateAreaDeNegocio();
        }

        private void ValidateTipoAgencia() {
            RuleFor(r => r.TipoDeAgencia_Id)
                .NotNull().WithMessage("'Tipo de Agência' é obrigatório.");
        }

        private void ValidateTipoDeSeguro() {
            RuleFor(r => r.TipoDeSeguro_Id).NotNull().WithMessage("'Tipo de Seguro' é obrigatório.");
        }

        private void ValidateRamoDeSeguro() {
            RuleFor(r => r.TipoDeProduto_Id).NotNull().WithMessage("'Ramo de Seguro' é obrigatório.");
        }

        private void ValidateAreaDeNegocio() {
            RuleFor(r => r.AreaDeNegocio_Id).NotNull().WithMessage("'Área de Negócio' é obrigatório.");
        }
    }
}
