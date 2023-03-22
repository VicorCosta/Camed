using Camed.SSC.Application.Requests.Solicitacao.Validators.SalvarArquivo;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivo
{
    public class FormatarArquivoSolicitacaoCommand : CommandBase, IRequest<IResult>
    {
        public List<AnexoDeSolicitacaoViewModel> FormFiles;
        public int Solicitacao_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new FormatarArquivoSolicitacaoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
