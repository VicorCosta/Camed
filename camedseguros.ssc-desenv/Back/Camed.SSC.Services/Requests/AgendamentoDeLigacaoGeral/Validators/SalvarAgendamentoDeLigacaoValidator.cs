using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgendamentoDeLigacaoGeralValidator : AbstractValidator<SalvarAgendamentoDeLigacaoGeralCommand>
    {
        public SalvarAgendamentoDeLigacaoGeralValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.DataAgendamento)
                .NotEmpty().WithMessage("'Data do Agendamento' é obrigatório");
            

        }
    }
}
