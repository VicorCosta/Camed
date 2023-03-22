using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Camed.SSC.Application.Requests
{
    public class SalvarSeguradoCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string EmailSecundario { get; set; }
        public string TelefonePrincipal { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneAdicional { get; set; }
        public string Contato { get; set; }
        public VinculoBNB VinculoBNB { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarSeguradoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
