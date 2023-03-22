using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarInboxValidator : AbstractValidator<SalvarInboxCommand>
    {
        public SalvarInboxValidator()
        {
            ValidateInbox();
        }

        private void ValidateInbox()
        {
            RuleFor(r => r.RemetenteId).GreaterThan(0).WithMessage("'Remetente' da mensagem é inválido.");
            RuleFor(p => p.Assunto)
                .NotEmpty().WithMessage("O 'assunto' é obrigatório")
                /*.NotNull().WithMessage("O 'assunto' é obrigatório")*/
                .MaximumLength(100).WithMessage("Tamanho máximo de 'Assunto' é de 100 caracteres");

            /*RuleFor(p => p.Texto)
                .NotEmpty().WithMessage("O 'texto' é obrigatório")
                .NotNull().WithMessage("O 'texto' é obrigatório");*/

            RuleFor(p => p.Destinatarios)
                .NotEmpty().WithMessage("É necessário um destinatário");
                /*.NotNull().WithMessage("É necessário um destinatário");*/
        }
    }
}
