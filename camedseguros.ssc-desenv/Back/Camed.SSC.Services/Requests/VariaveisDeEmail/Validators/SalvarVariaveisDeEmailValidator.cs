﻿using Camed.SSC.Core;
using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Camed.SSC.Application.Requests
{
  public class SalvarVariaveisDeEmailValidator : AbstractValidator<SalvarVariaveisDeEmailCommand>
    {
       public SalvarVariaveisDeEmailValidator()
        {
            ValidateNome();
        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
               .NotEmpty().WithMessage("'Nome' é obrigatório")
               .NotNull().WithMessage("'Nome' é obrigatório")
               .MaximumLength(100).WithMessage("Tamanho máximo de 'Nome' é de 100 caracteres");


        }
    }
}
