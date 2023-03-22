using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirAcaoDeAcompanhamentoCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new ExcluirAcaoDeAcompanhamentoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
