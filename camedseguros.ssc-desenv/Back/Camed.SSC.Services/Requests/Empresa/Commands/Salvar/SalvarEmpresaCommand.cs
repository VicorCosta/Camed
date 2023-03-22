using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{


    public class TipoSeguroInputModel
    {
        public int TipoSeguro_Id { get; set; }
        public bool Permitido_Abrir { get; set; }
    }


    public class SalvarEmpresaCommand : CommandBase, IRequest<IResult>
    {
        public int Id {get; set;}

        public string Nome { get; set; }

        public List<TipoSeguroInputModel> TiposDeSeguro { get; set; } = new List<TipoSeguroInputModel>();

        public override bool IsValid()
        {
            ValidationResult = new SalvarEmpresaValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
