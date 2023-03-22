using Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivoBNB;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.SalvarArquivoBNB
{
    public class SalvarArquivoBNBValidator : AbstractValidator<SalvarArquivoBNBCommand>
    {
        public SalvarArquivoBNBValidator()
        {
            ValidateAnexosBNB();
        }
        private void ValidateAnexosBNB()
        {
            RuleFor(p => p.anexos.Count)
                .NotEqual(0).WithMessage("É necessário ter pelo menos um arquivo");
        }
    }
}
