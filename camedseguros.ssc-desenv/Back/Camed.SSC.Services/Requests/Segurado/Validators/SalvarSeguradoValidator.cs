using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarSeguradoValidator : AbstractValidator<SalvarSeguradoCommand>
    {
        public SalvarSeguradoValidator()
        {
            ValidateNome();

        }

        private void ValidateNome()
        {      
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("Descricao é obrigatorio")
                .NotNull().WithMessage("Descricao é obrigatorio")
                .MaximumLength(400).WithMessage("Tamanho máximo de caractere do 'Nome' é de 400");

            RuleFor(r => r.CpfCnpj)
              .NotEmpty().WithMessage("Descricao é obrigatorio")
              .NotNull().WithMessage("Descricao é obrigatorio")
              .MaximumLength(28).WithMessage("Tamanho máximo de caractere do Cpf/Cnpj é de 28");

            RuleFor(r => r.Email)
              .MaximumLength(400).WithMessage("Tamanho máximo de caractere do Email é de 400");

            RuleFor(r => r.TelefonePrincipal)
              .MaximumLength(80).WithMessage("Tamanho máximo de caractere da descrição é de 80");

            RuleFor(r => r.TelefoneCelular)
              .MaximumLength(40).WithMessage("Tamanho máximo de caractere do Telefone-Celular é de 40");

            RuleFor(r => r.TelefoneAdicional)
             .MaximumLength(40).WithMessage("Tamanho máximo de caractere do Telefone Adicional é de 40");

            RuleFor(r => r.EmailSecundario)
             .MaximumLength(400).WithMessage("Tamanho máximo de caractere do Email secundário é de 400");

            RuleFor(r => r.Contato)
              .MaximumLength(50).WithMessage("Tamanho máximo de caractere do contato é de 50");

        }
    }
}
