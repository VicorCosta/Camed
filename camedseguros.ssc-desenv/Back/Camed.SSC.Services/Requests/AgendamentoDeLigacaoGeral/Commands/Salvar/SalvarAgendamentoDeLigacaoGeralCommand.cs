using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAgendamentoDeLigacaoGeralCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public string DataAgendamento { get; set; }
        public string Motivo { get; set; }
        public string DataLigacao { get; set; }
        public int TipoRetornoLigacao { get; set; }
        public int numero { get; set; }
        //public int Solicitacao_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarAgendamentoDeLigacaoGeralValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
