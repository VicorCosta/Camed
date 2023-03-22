using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarParametroSistemaCommand : CommandBase, IRequest<IResult>
    {

        public int Id { get; set; }
        public string Parametro { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; }
        public int? VariaveisDeEmail_Id { get; set; }
        public int? TipoDeParametro_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarParametroSistemaValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
