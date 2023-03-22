using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMapeamentoAreaDeNegocioCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public int? TipoDeAgencia_Id { get; set; }
        public int? TipoDeSeguro_Id { get; set; }
        public int? OperacaoDeFinanciamento { get; set; }
        public int? TipoDeProduto_Id { get; set; }
        public int? VinculoBNB_Id { get; set; }
        public int? AreaDeNegocio_Id { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new SalvarMapeamentoAreaDeNegocioValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
