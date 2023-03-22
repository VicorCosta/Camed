using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeSeguroValidator : AbstractValidator<SalvarTipoDeSeguroCommand>
    {
        public SalvarTipoDeSeguroValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
        }
    }
}
