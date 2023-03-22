using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.AdicionarAcompanhamento
{
    public class AdicionarAcompanhamentoCommandHandler : HandlerBase, IRequestHandler<AdicionarAcompanhamentoCommand, IResult>
    {
        private readonly IAcompanhamentoAppService acompanhamentoAppService;
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService identityAppService;

        public AdicionarAcompanhamentoCommandHandler(IAcompanhamentoAppService acompanhamentoAppService, 
            IUnitOfWork uow, IIdentityAppService identityAppService)
        {
            this.acompanhamentoAppService = acompanhamentoAppService;
            this.uow = uow;
            this.identityAppService = identityAppService;
        }

        public async Task<IResult> Handle(AdicionarAcompanhamentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var acompanhamento = request.acompanhamento;

                var usuarioLogado = uow.GetRepository<Usuario>().GetByIdAsync(2297/*identityAppService.Identity.Id*/).Result;

                

                if (acompanhamento == null)
                    throw new ApplicationException("Sem informação de acompanhamento a ser adicionado");

                var solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(x => x.Id == acompanhamento.Solicitacao_Id).Result;

                acompanhamento.Anexos.ForEach(w => w.Caminho = "anexos/" + solicitacao.Agencia.Codigo 
                + '/' + solicitacao.Numero + "/" + w.Nome);

                acompanhamentoAppService.AdicionarAcompanhamento(acompanhamento, usuarioLogado.Login);

                
                result.Payload = solicitacao;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
