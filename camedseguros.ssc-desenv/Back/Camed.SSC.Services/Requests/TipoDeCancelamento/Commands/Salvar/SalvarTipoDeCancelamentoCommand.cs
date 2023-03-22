using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeCancelamentoCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public int GrupoAgencia { get; set; }
        public string Descricao { get; set; }



        public override bool IsValid()
        {
            ValidationResult = new SalvarTipoDeCancelamentoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
