using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests.Solicitacao.Command.SolicitacaoBNB;
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

namespace Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivoBNB
{
    public class SalvarArquivoBNBCommandHandler : HandlerBase, IRequestHandler<SalvarArquivoBNBCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarArquivoBNBCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarArquivoBNBCommand request, CancellationToken cancellationToken)
        {
            foreach (var file in request.anexos) 
            {
                file.Solicitacao_Id = 250839;
                var mensagem = await uow.GetRepository<Domain.Entities.Solicitacao>().GetByIdAsync(file.Solicitacao_Id);
                var caminhoDiretorioAnexos = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS").Result.Valor;

                var solicitacao = uow.GetRepository<Domain.Entities.Solicitacao>().QueryFirstOrDefaultAsync(predicate: w => w.Id == file.Solicitacao_Id,
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

                caminhoDiretorioAnexos += $"\\anexos\\solicitacao\\{solicitacao.Agencia.Codigo}\\{solicitacao.Numero}";
                if (!Directory.Exists(caminhoDiretorioAnexos))
                {
                    Directory.CreateDirectory(caminhoDiretorioAnexos);
                }

                file.Caminho = $"{caminhoDiretorioAnexos}\\{file.NomeDoArquivo}{file.Extensao}";

                if (File.Exists(file.Caminho))
                {
                    var novoNome = $"{ file.NomeDoArquivo }{Path.GetRandomFileName()}";
                    file.Caminho = $"{caminhoDiretorioAnexos}\\{novoNome}{file.Extensao}";
                    file.NomeDoArquivo = novoNome;
                }

                var novoAnexoSolicitacao = new AnexoDeSolicitacao();
                novoAnexoSolicitacao.Caminho = file.Caminho;
                novoAnexoSolicitacao.Nome = file.NomeDoArquivo;
                novoAnexoSolicitacao.Solicitacao = solicitacao;
                solicitacao.Anexos.Add(novoAnexoSolicitacao);

                await uow.GetRepository<Domain.Entities.Solicitacao>().UpdateAndSaveAsync(solicitacao);
                uow.Commit();

                using (var stream = new FileStream (file.Caminho, FileMode.Create))
                {
                    byte[] byteArray = Convert.FromBase64String(file.ConteudoBase64);
                    stream.Write(byteArray);
                }
            }
            return await Task.FromResult(result);
        }
    }
}
