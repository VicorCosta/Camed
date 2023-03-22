using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgenciaTipoDeAgenciaCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}

        public int? AgenciaId { get; set; }

        public int? TipoDeAgenciaId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarAgenciaTipoDeAgenciaValidator().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
