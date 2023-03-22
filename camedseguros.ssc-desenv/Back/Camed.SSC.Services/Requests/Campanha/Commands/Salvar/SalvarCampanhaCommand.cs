using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCampanhaCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}

        public string Nome { get; set; }

        public bool Ativo { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new SalvarCampanhaValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
