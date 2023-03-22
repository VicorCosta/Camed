using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeProdutoCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int Situacao { get; set; }
        public int SlaMaximo { get; set; }
        public bool UsoInterno { get; set; }
        public int SituacaoRenovacao { get; set; }
        public string DescricaoSasParaTipoDeProduto { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarTipoDeProdutoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
