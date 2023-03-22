using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Interfaces;
using MediatR;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFluxoDeOperacaoCommand : CommandBase, IRequest<IResult>
    {
        public int Id { get; set; }
        public int? OrdemFluxo { get; set; }
        public int? OrdemFluxo2 { get; set; }
        public int? SituacaoAtual_Id { get; set; }
        public int? Acao_Id { get; set; }
        public int? ProximaSituacao_Id { get; set; }
        public int? Grupo_Id { get; set; }
        public bool PermiteEnvioDeArquivo { get; set; }
        public bool ExigeEnvioDeArquivo { get; set; }
        public bool EnviaEmailSolicitante { get; set; }
        public bool EnviaEmailAtendente { get; set; }
        public bool ExigeObservacao { get; set; }
        public bool EnviaEmailAoSegurado { get; set; }
        public int? ParametrosSistema_Id { get; set; }
        public bool EnviaSMSAoSegurado { get; set; }
        public int? ParametroSistemaSMS_Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SalvarFluxoDeOperacaoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
