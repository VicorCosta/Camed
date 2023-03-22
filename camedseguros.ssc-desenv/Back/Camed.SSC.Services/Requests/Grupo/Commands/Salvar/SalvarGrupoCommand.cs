using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarGrupoCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool SempreVisualizarObservacao { get; set; }
        public bool AtribuirAtendente { get; set; }
        public bool AtribuirOperador { get; set; }
        public bool CancelarSolicitacao { get; set; }
        public int[] Menus { get; set; }
        public int[] Subgrupos { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new SalvarGrupoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
