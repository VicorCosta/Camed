using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgendamentoDeLigacaoValidator : AbstractValidator<SalvarAgendamentoDeLigacaoCommand>
    {
        public SalvarAgendamentoDeLigacaoValidator()
        {
            ValidateMotivo();
            ValidateSolicitacao();
            ValidateData();
        }

        private void ValidateMotivo()
        {

            RuleFor(r => r.Motivo)
                .NotNull().WithMessage("'Motivo' é obrigatório")
                .MaximumLength(200).WithMessage("'Motivo' deve possuir no máximo 200 caracteres");
        }

        private void ValidateSolicitacao()
        {

            RuleFor(r => r.NSolicitacao)
                .NotEmpty().WithMessage("'N° Solicitação' é obrigatório")
                .NotNull().WithMessage("'N° Solicitação' é obrigatório");
        }
        private void ValidateData()
        {

            RuleFor(r => r.DataAgendamento)
               .NotEmpty().WithMessage("'Data do Agendamento' é obrigatória");

        }
    }
}
