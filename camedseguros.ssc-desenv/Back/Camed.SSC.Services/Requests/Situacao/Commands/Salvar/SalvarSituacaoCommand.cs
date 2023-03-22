
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarSituacaoCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public int? TempoSLA { get; set; }
        public bool EFimFluxo { get; set; }
        public bool PendenciaCliente { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarSituacaoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
