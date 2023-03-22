using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTextoPersonalizadoParametroValidator : AbstractValidator<SalvarTextoPersonalizadoParametroCommand>
    {
        public SalvarTextoPersonalizadoParametroValidator()
        {
            ValidateTipoSeguro();
            ValidateTipoProduto();
            ValidateTexto();
        }

        private void ValidateTipoSeguro()
        {
            RuleFor(r => r.TipoDeSeguro_Id)
                .NotEmpty().WithMessage("'Tipo de Seguro' é obrigatório");
        }
        private void ValidateTipoProduto()
        {
            RuleFor(r => r.TipoDeProduto_Id)
                .NotEmpty().WithMessage("'Ramo de Seguro' é obrigatório");
        }
        private void ValidateTexto()
        {
            RuleFor(r => r.Texto)
                .NotNull().WithMessage("'Texto/Html' é obrigatório");
        }


    }
}
