using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMapeamentoDeAtendenteCommand : CommandBase, IRequest<IResult>
    {

        public int Id { get; set; }
        public int? TipoDeSeguro_Id { get; set; }
        public int? Agencia_Id { get; set; }
        public int? GrupoAgencia_Id { get; set; }
        public int? AreaDeNegocio_Id { get; set; }
        public int? Atendente_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarMapeamentoDeAtendenteValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
