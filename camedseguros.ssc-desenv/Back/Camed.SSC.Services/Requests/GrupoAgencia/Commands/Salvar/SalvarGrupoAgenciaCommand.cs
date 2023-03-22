using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarGrupoAgenciaCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}

        public string Nome { get; set; }



        public override bool IsValid()
        {
            ValidationResult = new SalvarGrupoAgenciaValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
