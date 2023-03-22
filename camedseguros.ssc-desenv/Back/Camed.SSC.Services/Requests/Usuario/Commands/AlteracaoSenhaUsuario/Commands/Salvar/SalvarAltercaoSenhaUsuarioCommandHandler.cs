using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Commands;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Core.Security;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace Camed.SSC.Application.Requests {
    public class SalvarSenhaUsuarioCommandHandler : HandlerBase, IRequestHandler<SalvarAlteracaoSenhaUsuarioCommand, IResult> {
        private readonly IUnitOfWork uow;
        private readonly IMediator mediator;

        public SalvarSenhaUsuarioCommandHandler(IUnitOfWork uow, IMediator mediator) {
            this.uow = uow;
            this.mediator = mediator;
        }

        public async Task<IResult> Handle(SalvarAlteracaoSenhaUsuarioCommand request, CancellationToken cancellationToken) {

            if(!request.IsValid()) {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var criptoSenha = Encryption.CreateMD5Hash(request.senhaAtual);

            Usuario _usuario = await uow.GetRepository<Usuario>().QueryFirstOrDefaultAsync(u => u.Login == request.Username && u.Senha == criptoSenha, null);

            if(_usuario == null) {
                throw new ApplicationException("Senha Atual informada não confere com o cadastro, verifique e tente novamente.");
            } else {
                if(!VerificarForcaDaSenha(request.SenhaNova)) {
                    if(request.SenhaNova.Any(c => char.IsWhiteSpace(c))) {
                        throw new ApplicationException("A senha não pode conter espaços.");
                    }
                    throw new ApplicationException("Nova Senha não atende aos padrões. Tente novamente.");
                }

                if(!IsPassworEquals(request.SenhaNova, request.SenhaConfirmar)) {
                    throw new ApplicationException("As senhas informadas não coincidem. Tente novamente.");
                }

                if (request.senhaAtual == request.SenhaConfirmar)
                {
                    throw new ApplicationException("A nova senha não pode ser igual a senha já registrada no sistema. Tente novamente.");
                }

                _usuario.Senha = Encryption.CreateMD5Hash(request.SenhaNova);
                uow.GetRepository<Domain.Entities.Usuario>().Update(_usuario);
            }
            await uow.CommitAsync();
            return await Task.FromResult(result);
        }

        public bool IsPassworEquals(string passwordAntiga, string passwordAtual) {
            bool resp = passwordAntiga.Equals(passwordAtual);
            if(!resp)
                return false;

            return true;
        }



        private Boolean VerificarForcaDaSenha(string senha) {

            if(senha.Length < 6)
                return false;


            if(senha.Any(c => char.IsWhiteSpace(c)))
                return false;

            if(!senha.Any(c => char.IsDigit(c)))
                return false;

            if(!senha.Any(c => char.IsUpper(c)))
                return false;

            if(!senha.Any(c => char.IsLower(c)))
                return false;

            if(!senha.Any(c => char.IsPunctuation(c)))
                return false;

            return true;

        }



    }
}
