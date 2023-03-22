using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirEmpresaCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public ICollection<int> TiposDeSeguro { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new ExcluirEmpresaValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
