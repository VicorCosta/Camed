using Camed.SSC.Core;
using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarUsuarioValidator : AbstractValidator<SalvarUsuarioCommand>
    {
        public SalvarUsuarioValidator()
        {
            ValidateMatricula();
            ValidateLogin();
            ValidateNome();

            ValidateEmail();
            ValidateEmpresa();
            ValidateCPF();

            //validateGrupo();

            ValidateAgencia();
            ValidateGruposDeAgencias();
            ValidateAreasDeNegocio();
        }

        private void ValidateNome()
        {
            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("'Nome' é obrigatório");

        }

        private void ValidateLogin()
        {
            RuleFor(r => r.Login)
                .NotEmpty().WithMessage("'Login' é obrigatório")
                .MaximumLength(100).WithMessage("'Login' deve possuir no máximo 100 caracteres");
        }

        //async Task<bool> MatriculaUnica(SalvarUsuarioCommand dto, string matricula, System.Threading.CancellationToken cancellationToken) 
        //{
        //    SCC.Infrastructure.Data.Context.SSCContext context = new SCC.Infrastructure.Data.Context.SSCContext(new Microsoft.EntityFrameworkCore.DbContextOptions<SCC.Infrastructure.Data.Context.SSCContext>());
        //    var t = context.Usuarios.Any(a => a.Id != dto.Id);
        //    var t2 = context.Usuarios.FirstOrDefault();
        //    return context.Usuarios.Any(a => a.Id != dto.Id && a.Matricula.ToLower() == matricula.ToLower()) == false; 
        //}

        private void ValidateMatricula()
        {
            RuleFor(r => r.Matricula)
                .NotNull().WithMessage("'Matrícula' é obrigatória")
                .MaximumLength(50).WithMessage("'Matrícula' deve possuir no máximo 50 caracteres.");
                //.MustAsync(MatriculaUnica).WithMessage("'Matrícula' já cadastrada");
        }

        private void ValidateEmail()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("'Email' é obrigatório")
                .NotNull().WithMessage("'Email' é obrigatório");
                //.EmailAddress().WithMessage("'Email' em formato inválido");
        }

        private void ValidateEmpresa()
        {
            RuleFor(r => r.Empresa)
                .NotNull().WithMessage("'Empresa' é obrigatório");
        }

        private void validateGrupo()
        {
            RuleFor(r => r.Grupos)
                .NotNull().WithMessage("'Grupo' é obrigatório");
        }

        private void ValidateCPF()
        {
            RuleFor(r => r.CPF)
                .NotNull().WithMessage("'CPF' é obrigatório");
        }

        private void ValidateAgencia()
        {
            RuleFor(r => r.Agencia)
                .NotNull()
                .When(w => w.Grupos != null && w.Grupos.Contains(AppConstants.GrupoGerenteDeAgencia))
                .WithMessage("'Agência' é obrigatória quando 'Gerente de Agência' for um grupo selecionado");
        }

        private void ValidateGruposDeAgencias()
        {
            RuleFor(r => r.GruposAgencias)
                .NotNull()
                .When(w => w.Grupos != null && (w.Grupos.Contains(AppConstants.GrupoGerente) || w.Grupos.Contains(AppConstants.GrupoAtendente)))
                .WithMessage("'Grupo Agência' é obrigatória quando um dos grupos 'Gerente' ou 'Atendente' estiverem selecionados");
        }

        private void ValidateAreasDeNegocio()
        {
            RuleFor(r => r.AreasDeNegocio)
                .NotNull()
                .When(w => w.Grupos != null && (w.Grupos.Contains(AppConstants.GrupoGerente) || w.Grupos.Contains(AppConstants.GrupoAtendente)))
                .WithMessage("'Áreas de Negócio' é obrigatória quando um dos grupos 'Gerente' ou 'Atendente' estiverem selecionados");
        }
    }
}
