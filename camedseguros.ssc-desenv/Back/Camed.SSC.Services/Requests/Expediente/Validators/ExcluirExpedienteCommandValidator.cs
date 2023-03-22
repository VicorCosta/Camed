using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirExpedienteCommandValidator : AbstractValidator<ExcluirExpedienteCommand>
    {
        public ExcluirExpedienteCommandValidator()
        {
            ValidateId();
        }

        private void ValidateId()
        {
          RuleFor(r=>r.Id).GreaterThan(0).WithMessage("'Id' deve ser maior que zero");
          
        }
    }
}
