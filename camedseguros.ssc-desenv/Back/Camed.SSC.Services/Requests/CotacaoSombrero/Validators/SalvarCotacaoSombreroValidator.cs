using FluentValidation;

namespace Camed.SSC.Application.Requests
{
    public class SalvarCotacaoSombreroValidator : AbstractValidator<SalvarCotacaoSombreroCommand>
    {
        public SalvarCotacaoSombreroValidator()
        {
            ValidateCep();
            ValidateCod();
            ValidateTipSub();
            ValidateTipoCul();
            ValidateValorCust();
            ValidateNivCob();
            ValidateAreaPla();
            ValidateCot();
            ValidateUniPes();
        }

        private void ValidateCep()
        {
            RuleFor(r => r.CepAreaDeRisco)
                .NotEmpty().WithMessage("'Cep' é obrigatório")
                .NotNull().WithMessage("'Cep' é obrigatório");

        }

        private void ValidateCod()
        {
            RuleFor(r => r.CodigoProduto)
                .NotEmpty().WithMessage("'Código Produto' é obrigatório")
                .NotNull().WithMessage("'Código Produto' é obrigatório");

        }
        private void ValidateTipoCul()
        {
            RuleFor(r => r.CodigoCultivo)
                .NotEmpty().WithMessage("'Código Cultivo' é obrigatório")
                .NotNull().WithMessage("'Código Cultivo' é obrigatório");

        }
        private void ValidateNivCob()
        {
            RuleFor(r => r.NivelCobertura)
                .NotEmpty().WithMessage("'Nível Cobertura' é obrigatório")
                .NotNull().WithMessage("'Nível Cobertura' é obrigatório");

        }
        private void ValidateAreaPla()
        {
            RuleFor(r => r.AreaTotal)
                .NotEmpty().WithMessage("'Área Total' é obrigatório")
                .NotNull().WithMessage("'Área Total' é obrigatório");

        }
        private void ValidateCot()
        {
            RuleFor(r => r.TipoCotacao)
                .NotEmpty().WithMessage("'Tipo Cotação' é obrigatório")
                .NotNull().WithMessage("'Tipo Cotação' é obrigatório");

        }
        private void ValidateUniPes()
        {
            RuleFor(r => r.UnidadePesoCultivo)
                .NotEmpty().WithMessage("'Unidade Peso Cultivo' é obrigatório")
                .NotNull().WithMessage("'Unidade Peso Cultivo' é obrigatório");

        }
        private void ValidateValorCust()
        {
            RuleFor(r => r.ValorCusteio_Preco)
                .NotEmpty().WithMessage("'Valor Custeio' é obrigatório")
                .NotNull().WithMessage("'Valor Custeio' é obrigatório");

        }
        private void ValidateTipSub()
        {
            RuleFor(r => r.TipoSubvencao)
                .NotEmpty().WithMessage("'Tipo Subvenção' é obrigatório")
                .NotNull().WithMessage("'Tipo Subvenção' é obrigatório");

        }
    }
}

