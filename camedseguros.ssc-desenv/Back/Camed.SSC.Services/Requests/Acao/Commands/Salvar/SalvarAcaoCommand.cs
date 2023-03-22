using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAcaoCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }
        public int Papel { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new SalvarAcaoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
