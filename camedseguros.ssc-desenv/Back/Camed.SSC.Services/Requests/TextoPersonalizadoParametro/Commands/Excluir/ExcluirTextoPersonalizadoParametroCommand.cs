using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirTextoPersonalizadoParametroCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ExcluirTextoPersonalizadoParametroValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
