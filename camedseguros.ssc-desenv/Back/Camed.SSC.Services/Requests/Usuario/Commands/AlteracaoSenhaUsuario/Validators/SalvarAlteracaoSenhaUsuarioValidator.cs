using Camed.SSC.Core;
using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAlteracaoSenhaUsuarioValidator : AbstractValidator<SalvarAlteracaoSenhaUsuarioCommand>
    {
        public SalvarAlteracaoSenhaUsuarioValidator()
        {
            ValidateSenha();
           
        }

        private void ValidateSenha()
        {
            RuleFor(r => r.senhaAtual)
                .NotNull().WithMessage("'Senha Atual' é obrigatório.");


            RuleFor(r => r.senhaAtual)
                .NotEmpty().WithMessage("'Senha Atual' tem formato inválido.").When(r => r.senhaAtual != null);


            RuleFor(r => r.SenhaNova)
                .NotEmpty().WithMessage("'Nova Senha' tem formato inválido.").When(r => r.SenhaNova != null)
                .MaximumLength(100).WithMessage("'Login' deve possuir no máximo 100 caracteres.");

            RuleFor(r => r.SenhaNova)
                .NotNull().WithMessage("'Nova Senha' é obrigatório.");

                        

            RuleFor(r => r.SenhaConfirmar)
               .NotEmpty().WithMessage("'Confirmar Senha' tem formato inválido.").When(r => r.SenhaConfirmar != null);

            RuleFor(r => r.SenhaConfirmar)
                .NotNull().WithMessage("'Confirmar Senha' é obrigatório.");
        }

    }
}
