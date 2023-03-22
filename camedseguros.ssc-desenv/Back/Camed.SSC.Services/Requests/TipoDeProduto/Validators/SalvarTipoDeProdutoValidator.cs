using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarTipoDeProdutoValidator : AbstractValidator<SalvarTipoDeProdutoCommand>
    {
        public SalvarTipoDeProdutoValidator()
        {
            ValidateNome();
            validateSituacaoInicial();
            validateDescricaoSasParaTipoDeProduto();

        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório")
                .NotNull().WithMessage("'Nome' é obrigatório")
                .MaximumLength(150).WithMessage("Tamanho máximo de 'Nome' é de 150 caracteres");
        }

        private void validateSituacaoInicial()
        {
            RuleFor(r => r.Situacao)
                .NotEmpty().WithMessage("'Situacao' é obrigatório")
                .NotNull().WithMessage("'Situacao' é obrigatório");
        }

        private void validateDescricaoSasParaTipoDeProduto()
        {
            RuleFor(r => r.DescricaoSasParaTipoDeProduto)
                .MaximumLength(100).WithMessage("Tamanho máximo de 'Descrição' é de 100 caracteres");
        }

    }
}
