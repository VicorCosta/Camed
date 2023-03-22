using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFeriadoCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}
        public string Data { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public int Municipio { get; set; }
        public string Descricao { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarFeriadoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
