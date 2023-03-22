using Camed.SSC.Application.Requests.TiposdeParametros.Validators;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Requests.TiposdeParametros.Commands.Excluir
{
    public class ExcluirTiposdeParametrosCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new ExcluirTiposdeParametrosValidator().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
