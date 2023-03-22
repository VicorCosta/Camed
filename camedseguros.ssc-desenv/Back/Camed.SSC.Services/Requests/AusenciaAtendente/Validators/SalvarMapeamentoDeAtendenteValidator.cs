using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAusenciaAtendenteValidator : AbstractValidator<SalvarAusenciaAtendenteCommand>
    {
        public SalvarAusenciaAtendenteValidator()
        {
            ValidateDataI();
            ValidateDataF();
            ValidateAtendente();
        }
        private void ValidateDataI()
        {
            RuleFor(r => r.DataInicioAusencia).NotEmpty()
                .NotNull().WithMessage("'Data Inicio' é obrigatória");
        }
        private void ValidateDataF()
        {
            RuleFor(r => r.DataFinalAusencia).NotEmpty()
                .NotNull().WithMessage("'Data Final' é obrigatória");
        }
        private void ValidateAtendente()
        {
            RuleFor(r => r.Atendente_Id)
                .NotNull().WithMessage("'Atendente' é obrigatória");
        }
    }
}
