using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFluxoDeOperacaoValidator : AbstractValidator<SalvarFluxoDeOperacaoCommand>
    {
        public SalvarFluxoDeOperacaoValidator()
        {
            ValidateOrdemFluxo();
            ValidateOrdemFluxo2();
            ValidateProximaSituacao();
            ValidateAcao();
            ValidateSituacaoAtual();
            ValidateGrupo();
            ValidateParametroEmail();
            ValidateParametroSMS();
            ValidateOrdemFluxoVazio();
        }

        private void ValidateOrdemFluxo()
        {
            RuleFor(r => r.OrdemFluxo)
                .GreaterThan(0).When(w => w.OrdemFluxo.HasValue).WithMessage("'Ordem do fluxo' deve ser maior que zero.");
        }

        private void ValidateOrdemFluxoVazio()
        {
            RuleFor(r => r.OrdemFluxo)
                .NotNull().WithMessage("'Ordem Fluxo' é obrigatório");
        }

        private void ValidateOrdemFluxo2()
        {
            RuleFor(r => r.OrdemFluxo2)
                .GreaterThan(0).When(w => w.OrdemFluxo2.HasValue).WithMessage("'Ordem do fluxo 2' deve ser maior que zero.");
        }

        private void ValidateSituacaoAtual()
        {
            RuleFor(r => r.SituacaoAtual_Id)
                .NotNull().WithMessage("'Stiuação Atual' é obrigatória");
        }

        private void ValidateAcao()
        {
            RuleFor(r => r.Acao_Id)
                .NotNull().WithMessage("'Ação' é obrigatório");
        }

        private void ValidateProximaSituacao()
        {
            RuleFor(r => r.ProximaSituacao_Id)
                .NotNull().WithMessage("'Próxima Situação' é obrigatória");
        }

        private void ValidateGrupo()
        {
            RuleFor(r => r.Grupo_Id)
                .NotNull().WithMessage("'Grupo' é obrigatório");
        }

        private void ValidateParametroEmail()
        {
            RuleFor(r => r.ParametrosSistema_Id)
                .NotNull().When(w => w.EnviaEmailAoSegurado == true).WithMessage("'Modelo do email ao Segurado' é obrigatório quando 'Enviar email ao Segurado' estiver selecionado");
        }

        private void ValidateParametroSMS()
        {
            RuleFor(r => r.ParametroSistemaSMS_Id)
                .NotNull().When(w => w.EnviaSMSAoSegurado == true).WithMessage("'Modelo do SMS ao Segurado' é obrigatório quando 'Enviar SMS ao Segurado' estiver selecionado");
        }
    }
}
