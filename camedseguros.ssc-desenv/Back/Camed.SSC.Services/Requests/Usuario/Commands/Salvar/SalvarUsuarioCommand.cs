using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Camed.SSC.Application.Requests
{
    public class SalvarUsuarioCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public int? Empresa { get; set; }
        public bool Ativo { get; set; }
        public string Senha { get; set; }
        public int? Agencia { get; set; }
        public bool SenhaETemporaria { get; set; }
        public bool EnviarSLA { get; set; }
        public bool EhCalculista { get; set; }
        public int[] GruposAgencias { get; set; }
        public int[] Grupos { get; set; }
        public int[] AreasDeNegocio { get; set; }




        public override bool IsValid()
        {
            ValidationResult = new SalvarUsuarioValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
