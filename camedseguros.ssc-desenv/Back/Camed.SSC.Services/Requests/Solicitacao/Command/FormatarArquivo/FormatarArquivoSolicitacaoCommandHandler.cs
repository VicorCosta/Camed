using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivo
{
    public class FormatarArquivoSolicitacaoCommandHandler : HandlerBase, IRequestHandler<FormatarArquivoSolicitacaoCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public FormatarArquivoSolicitacaoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(FormatarArquivoSolicitacaoCommand request, CancellationToken cancellationToken)
        {
            foreach (var anexo in request.FormFiles)
            {
                anexo.Solicitacao_Id = request.Solicitacao_Id;
                var mensagem = await uow.GetRepository<Domain.Entities.Solicitacao>().GetByIdAsync(anexo.Solicitacao_Id);
                
                if(mensagem == null)
                {
                    throw new ApplicationException("Mensagem não encontrada");
                }

                var caminhoDiretorioAnexos = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS").Result.Valor;

                var solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(predicate: w => w.Id == anexo.Solicitacao_Id, 
                    includes: new[] {
                        "Atendente",
                        "Operador",
                        "Solicitante",
                        "Agencia",
                        "Produtor",
                        "TipoDeProduto",
                        "CanalDeDistribuicao",
                        "TipoDeSeguro",
                        "Segurado",
                        "Segmento",
                        "AreaDeNegocio",
                        "SituacaoAtual",
                        "Seguradora",
                        "Ramo",
                        "TipoSeguroGS",
                        "FL_Forma_Pagamento_1a",
                        "FL_Forma_Pagamento_Demais",
                        "GrupoDeProducao",
                        "TipoDeCategoria",
                        "TipoDeCancelamento",
                        "MotivoEndossoCancelamento",
                        "MotivoRecusa",
                        "AgenciaConta",
                        "SeguradoraCotacao",
                        "Anexos",
                        "Acompanhamentos",
                        "AgendamentosDeLigacao",
                        "CheckList",
                        "Indicacoes",
                        "Checkins",
                    }).Result;

                caminhoDiretorioAnexos = $"\\anexos\\solicitacao\\{solicitacao.Agencia.Codigo}\\{solicitacao.Numero}";
                if (!Directory.Exists(caminhoDiretorioAnexos))
                {
                    Directory.CreateDirectory(caminhoDiretorioAnexos);
                }

                anexo.Caminho = $"{caminhoDiretorioAnexos}\\{anexo.Nome}{anexo.Extensao}";

                if (File.Exists(anexo.Caminho))
                {
                    var novoNome = $"{ anexo.Nome }{ Path.GetRandomFileName()}";
                    novoNome = novoNome.Substring(0,novoNome.Length - 4);
                    anexo.Caminho = $"{caminhoDiretorioAnexos}\\{novoNome}{anexo.Extensao}";
                    anexo.Nome = novoNome;
                }

                var novoAnexoSolicitacao = new AnexoDeSolicitacao();
                novoAnexoSolicitacao.Caminho = anexo.Caminho;
                novoAnexoSolicitacao.Nome = anexo.Nome;
                novoAnexoSolicitacao.Solicitacao = solicitacao;
                solicitacao.Anexos.Add(novoAnexoSolicitacao);

                await uow.GetRepository<Domain.Entities.Solicitacao>().UpdateAndSaveAsync(solicitacao);
                uow.Commit();

                using (var stream = new FileStream(anexo.Caminho, FileMode.Create))
                {
                    stream.Write(anexo.Arquivo);
                }
            }
            return await Task.FromResult(result);
        }
    }
}
