using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeDocumentoCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}
        public string Nome { get; set; }
        public int[] RamosDeSeguro { get; set; }

        public int? TipoMorte_id { get; set; }
        public int? Ordem { get; set; }
        public bool Obrigatorio { get; set; }
        public bool Ativo { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new SalvarTipoDeDocumentoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
