using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarFluxoDeOperacaoCommandHandler : HandlerBase, IRequestHandler<SalvarFluxoDeOperacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarFluxoDeOperacaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarFluxoDeOperacaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var situacaoAtual = await uow.GetRepository<Situacao>().GetByIdAsync(request.SituacaoAtual_Id.Value) ?? throw new ApplicationException("Situação atual não foi localizada na base de dados");
            var acao = await uow.GetRepository<AcaoDeAcompanhamento>().GetByIdAsync(request.Acao_Id.Value) ?? throw new ApplicationException("Ação não foi localizada na base de dados");
            var proximaSituacao = await uow.GetRepository<Situacao>().GetByIdAsync(request.ProximaSituacao_Id.Value) ?? throw new ApplicationException("Próxima situaão não foi localizada na base de dados");
            var grupo = await uow.GetRepository<Grupo>().GetByIdAsync(request.Grupo_Id.Value) ?? throw new ApplicationException("Grupo não foi localizado na base de dados");


            if (request.Id == 0)
            {
                Domain.Entities.MapeamentoAcaoSituacao mapeamento = 
                    new Domain.Entities.MapeamentoAcaoSituacao( situacaoAtual, 
                                                                acao, 
                                                                proximaSituacao, 
                                                                grupo, 
                                                                request.PermiteEnvioDeArquivo, 
                                                                request.ExigeEnvioDeArquivo, 
                                                                request.EnviaEmailSolicitante,
                                                                request.EnviaEmailAtendente,
                                                                request.ExigeObservacao, 
                                                                request.EnviaEmailAoSegurado, 
                                                                request.EnviaSMSAoSegurado );

                mapeamento.OrdemFluxo = request.OrdemFluxo;
                mapeamento.OrdemFluxo2 = request.OrdemFluxo2;

                if (request.EnviaEmailAoSegurado == true && request.ParametrosSistema_Id.HasValue)
                {
                    mapeamento.ParametrosSistema = await uow.GetRepository<ParametrosSistema>().GetByIdAsync(request.ParametrosSistema_Id.Value) ?? throw new ApplicationException("Paramêtro do sistema não foi localizado na base de dados");
                }

                if (request.EnviaSMSAoSegurado == true && request.ParametroSistemaSMS_Id.HasValue)
                {
                    mapeamento.ParametroSistemaSMS = await uow.GetRepository<ParametrosSistema>().GetByIdAsync(request.ParametroSistemaSMS_Id.Value) ?? throw new ApplicationException("Paramêtro do sistema para SMS não foi localizado na base de dados");
                }

                await uow.GetRepository<Domain.Entities.MapeamentoAcaoSituacao>().AddAsync(mapeamento);
            }
            else
            {
                var mapeamento = await uow.GetRepository<Domain.Entities.MapeamentoAcaoSituacao>().GetByIdAsync(request.Id) ?? throw new ApplicationException("O fluxo de operação não foi localizado na base de dados"); ;

                mapeamento.DefinirAcao(acao);
                mapeamento.DefinirGrupo(grupo);
                mapeamento.DefinirSituacaoAtual(situacaoAtual);
                mapeamento.DefinirProximaSituacao(proximaSituacao);

                mapeamento.OrdemFluxo = request.OrdemFluxo;
                mapeamento.OrdemFluxo2 = request.OrdemFluxo2;
                mapeamento.PermiteEnvioDeArquivo = request.PermiteEnvioDeArquivo;
                mapeamento.ExigeEnvioDeArquivo = request.ExigeEnvioDeArquivo;
                mapeamento.EnviaEmailSolicitante = request.EnviaEmailSolicitante;
                mapeamento.EnviaEmailAtendente = request.EnviaEmailAtendente;
                mapeamento.ExigeObservacao = request.ExigeObservacao;
                mapeamento.EnviaEmailAoSegurado = request.EnviaEmailAoSegurado;
                mapeamento.EnviaSMSAoSegurado = request.EnviaSMSAoSegurado;

                if (request.EnviaEmailAoSegurado == true && request.ParametrosSistema_Id.HasValue)
                {
                    mapeamento.ParametrosSistema = await uow.GetRepository<ParametrosSistema>().GetByIdAsync(request.ParametrosSistema_Id.Value) ?? throw new ApplicationException("Paramêtro do sistema não foi localizado na base de dados");
                }

                if (request.EnviaSMSAoSegurado == true && request.ParametroSistemaSMS_Id.HasValue)
                {
                    mapeamento.ParametroSistemaSMS = await uow.GetRepository<ParametrosSistema>().GetByIdAsync(request.ParametroSistemaSMS_Id.Value) ?? throw new ApplicationException("Paramêtro do sistema para SMS não foi localizado na base de dados");
                }

                uow.GetRepository<Domain.Entities.MapeamentoAcaoSituacao>().Update(mapeamento);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
