using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAusenciaAtendenteCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public DateTime DataInicioAusencia { get; set; }
        public DateTime DataFinalAusencia { get; set; }
        public int? Atendente_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarAusenciaAtendenteValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
