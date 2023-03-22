using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAreaDeNegocioCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}

        public string Nome { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarAreaDeNegocioValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
