using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarAlteracaoSenhaUsuarioCommand : CommandBase, IRequest<IResult>
    {     
        public string Id { get; set; }
        public string Username { get; set; }
        public string senhaAtual { get; set; }
        public string SenhaNova { get; set; }
        public string SenhaConfirmar { get; set; }
        
        public override bool IsValid()
        {
            ValidationResult = new SalvarAlteracaoSenhaUsuarioValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
