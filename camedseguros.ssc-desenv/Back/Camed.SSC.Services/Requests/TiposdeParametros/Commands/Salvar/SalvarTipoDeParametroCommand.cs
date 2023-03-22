using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.TiposdeParametros.Commands.Salvar
{
    public class SalvarTipoDeParametroCommand : CommandBase, IRequest<IResult>
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarTipoDeParametrosValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
