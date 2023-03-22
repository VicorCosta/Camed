using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTextoParametroSeguradoraCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }

        public int Seguradora_Id { get; set; }

        public string Texto { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new SalvarTextoParametroSeguradoraValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
