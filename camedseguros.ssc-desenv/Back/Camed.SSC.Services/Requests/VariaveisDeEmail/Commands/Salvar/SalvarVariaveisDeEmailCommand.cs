using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;

namespace Camed.SSC.Application.Requests
{
   public class SalvarVariaveisDeEmailCommand : CommandBase , IRequest<IResult>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? ParametroId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarVariaveisDeEmailValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
