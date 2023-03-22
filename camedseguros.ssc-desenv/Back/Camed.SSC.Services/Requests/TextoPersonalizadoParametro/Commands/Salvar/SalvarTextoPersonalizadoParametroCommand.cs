using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTextoPersonalizadoParametroCommand : CommandBase, IRequest<IResult>
    {

        public int Id { get; set; }
        public int? TipoDeSeguro_Id { get; set; }
        public int? TipoDeProduto_Id { get; set; }
        public string Texto { get; set;}

        public override bool IsValid()
        {
            ValidationResult = new SalvarTextoPersonalizadoParametroValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
