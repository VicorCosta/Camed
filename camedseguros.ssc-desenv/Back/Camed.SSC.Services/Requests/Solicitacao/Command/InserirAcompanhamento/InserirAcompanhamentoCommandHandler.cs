using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Application.Requests.Solicitacao.Command.InserirAcompanhamento;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.InserirAcompanhamento
{
    public class InserirAcompanhamentoCommandHandler : HandlerBase, IRequestHandler<InserirAcompanhamentoCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityAppService identity;
        private readonly ISolicitacaoAppService solicitacaoAppService;

        public InserirAcompanhamentoCommandHandler(IUnitOfWork uow, IIdentityAppService identity, ISolicitacaoAppService solicitacaoAppService)
        {
            this.uow = uow;
            this.identity = identity;
            this.solicitacaoAppService = solicitacaoAppService;
        }

        public async Task<IResult> Handle(InserirAcompanhamentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var grupo = uow.GetRepository<Grupo>().QueryFirstOrDefaultAsync(x => x.Nome.Equals("Atendente")).Result;
                var qtdeAcompanhamento = (await uow.GetRepository<Acompanhamento>().QueryAsync(acomp => acomp.Solicitacao_Id == request.Solicitacao_Id)).Count();
                var situacao = uow.GetRepository<Situacao>().QueryFirstOrDefaultAsync(situ => situ.Nome.Equals(request.Situacao.Nome)).Result;
                request.Situacao = situacao;

                Acompanhamento acompanhamento = new Acompanhamento();
                acompanhamento.Ordem = (++qtdeAcompanhamento);
                acompanhamento.DataEHora = DateTime.Now;
                acompanhamento.Atendente_Id = request.Atendente.Id;
                acompanhamento.Grupo = grupo;
                acompanhamento.TempoSLADef = request.TempoSLADef;
                acompanhamento.Situacao_Id = request.Situacao.Id;
                acompanhamento.Solicitacao_Id = request.Solicitacao_Id;

                //var usuarioLogado = uow.GetRepository<Usuario>().GetByIdAsync(identity.Identity.Id).Result;
                /*var solicitacao = request.Solicitacao;*/

                var solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>()
                .QueryFirstOrDefaultAsync(x => x.Id == request.Solicitacao_Id, includes: new[] { "Atendente", "Segurado", "Agencia", "AgenciaConta", "CheckList", "Indicacoes",
                "TipoDeProduto", "TipoDeSeguro", "SituacaoAtual", "TipoDeProduto", "TipoDeProduto.Situacao", "Solicitante", "Segmento","Operador", "Produtor",
                "CanalDeDistribuicao","AreaDeNegocio", "Anexos", "Acompanhamentos", "AgendamentosDeLigacao", "Seguradora", "Ramo",
"TipoSeguroGS", "FL_Forma_Pagamento_1a", "FL_Forma_Pagamento_Demais", "GrupoDeProducao", "TipoDeCategoria", "Checkins", "SeguradoraCotacao",
"Indicacoes","TipoDeCancelamento","MotivoEndossoCancelamento", "MotivoRecusa"}).Result ?? throw new ApplicationException("Solicitação não encontrada");
        
                var mapeamento = await uow.GetRepository<MapeamentoAcaoSituacao>().QueryFirstOrDefaultAsync(w => w.SituacaoAtual_Id == solicitacao.SituacaoAtual_Id && w.Acao_Id == request.Acao_Id) ?? throw new ApplicationException("A Ação não foi cadastradada na tela de Fluxo de Operações.");
                solicitacao.SituacaoAtual_Id = mapeamento.ProximaSituacao_Id;
                acompanhamento.Situacao_Id = mapeamento.ProximaSituacao_Id;

                uow.GetRepository<Domain.Entities.Solicitacao>().Update(solicitacao);
                await uow.GetRepository<Acompanhamento>().AddAndSaveAsync(acompanhamento);

                /*result.Payload = solicitacao;*/
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
