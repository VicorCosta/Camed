using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarMenuCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Rota { get; set; }

        public string Icone { get; set; }

        public int? MenuSuperior { get; set; }

        public string Ajuda { get; set; }

        //public List<Domain.Entities.Acao> Acoes { get; set; }
        public int[] Acoes { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new SalvarMenuValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
