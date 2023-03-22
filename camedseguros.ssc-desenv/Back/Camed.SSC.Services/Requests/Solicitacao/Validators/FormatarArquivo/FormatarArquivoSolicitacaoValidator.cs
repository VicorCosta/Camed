using Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Validators.SalvarArquivo
{
    public class FormatarArquivoSolicitacaoValidator : AbstractValidator<FormatarArquivoSolicitacaoCommand>
    {
        public FormatarArquivoSolicitacaoValidator()
        {
            ValidateAnexoDeSolicitacao();
        }

        private void ValidateAnexoDeSolicitacao()
        {
        }
    }
}
