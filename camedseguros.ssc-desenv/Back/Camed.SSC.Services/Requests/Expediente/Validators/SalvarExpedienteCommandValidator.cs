using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarExpedienteCommandValidator : AbstractValidator<SalvarExpedienteCommand>
    {
        public SalvarExpedienteCommandValidator()
        {
            ValidateDia();
            ValidateHoraInicialManha();
            ValidateHoraFinalManha();
            ValidateHoraInicialTarde();
            ValidateHoraFinalTarde();
        }

        private void ValidateHoraFinalTarde()
        {
    
            RuleFor(r => r.HoraFinalTardeDate).GreaterThan(r => r.HoraInicialTardeDate);
            RuleFor(r => r.HoraFinalTardeDate.Hour).LessThan(24);
        }

        private void ValidateHoraInicialTarde()
        {
            RuleFor(r => r.HoraInicialTardeDate).GreaterThan(r => r.HoraFinalManhaDate);
            RuleFor(r => r.HoraInicialTardeDate.Hour).LessThan(24);
        }

        private void ValidateHoraFinalManha()
        {
            RuleFor(r => r.HoraFinalManhaDate).GreaterThan(r => r.HoraInicialManhaDate);
            RuleFor(r => r.HoraFinalManhaDate.Hour).LessThan(24);
        }

        private void ValidateHoraInicialManha()
        {
            RuleFor(r => r.HoraInicialManhaDate).LessThan(r => r.HoraFinalManhaDate);
            RuleFor(r => r.HoraInicialManhaDate.Hour).LessThan(24);
        }

        private void ValidateDia()
        {
            RuleFor(r => r.Dia).InclusiveBetween(1, 7);
        }
    }
}
