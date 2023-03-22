using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarInboxCommandHandler : HandlerBase, IRequestHandler<SalvarInboxCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarInboxCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarInboxCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var idsDasMensagens = new List<int>();
            
            var remetente = await uow.GetRepository<Usuario>().GetByIdAsync(request.RemetenteId);
            if (remetente != null)
            {
                for (int i = 0; i < request.Destinatarios.Length; i++)
                {
                    var inbox = new Domain.Entities.Inbox();
                    var idDestinatario = request.Destinatarios[i];
                    var destinatario = await uow.GetRepository<Usuario>().GetByIdAsync(idDestinatario);
                    var solicitacao = await uow.GetRepository<Domain.Entities.Solicitacao>().GetByIdAsync((int)request.numeroSolicitacao);
                    
                    if (solicitacao == null)
                    {
                        throw new ApplicationException("Não existe este número de solicitação no banco de dados. Verifique e tente novamente.");
                    }
                    var teste = uow.GetRepository<Domain.Entities.Inbox>().QueryFirstOrDefaultAsync(inboxFinder => inboxFinder.Solicitacao_Id == request.numeroSolicitacao).Result;
                    if (uow.GetRepository<Domain.Entities.Inbox>().QueryFirstOrDefaultAsync(inboxFinder => inboxFinder.Solicitacao_Id == request.numeroSolicitacao).Result != null)
                    {
                        throw new ApplicationException("Este número de solicitação se encontra vinculado à outra mensagem de inbox. Verifique e tente novamente.");
                    }


                    if (destinatario != null)
                    {
                        inbox.UsuarioDestinatario = destinatario;
                        inbox.Texto = request.Texto;
                        inbox.Assunto = ((request.IdMensagemOriginal != 0) ? ("RESP: " + request.Assunto) : (request.Assunto));
                        inbox.Lida = false;
                        inbox.DataCriacao = DateTime.Now;
                        inbox.VisivelSaida = true;
                        inbox.VisivelEntrada = true;
                        inbox.MensagemResp_Id = request.IdMensagemOriginal;
                        inbox.Solicitacao_Id = request.numeroSolicitacao;
                        inbox.UsuarioRemetente = remetente;
                        inbox.Anexos = new List<AnexosDeInbox>();

                        await uow.GetRepository<Domain.Entities.Inbox>().AddAndSaveAsync(inbox);

                        foreach (var anexo in request.Anexos)
                        {
                            anexo.Inbox_Id = inbox.Id;
                            var caminhoDiretorioAnexos = ( await uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS")).Valor;
                            caminhoDiretorioAnexos += $"\\anexos\\inbox\\{anexo.Inbox_Id}";

                            if (!Directory.Exists(caminhoDiretorioAnexos))
                            {
                                Directory.CreateDirectory(caminhoDiretorioAnexos);
                            }

                            var caminhoFisicoAnexo = $"{caminhoDiretorioAnexos}\\{anexo.Nome}{anexo.Extensao}"; ;
                            anexo.Caminho = $"\\anexos\\inbox\\{anexo.Inbox_Id}\\{ anexo.Nome}{anexo.Extensao}";

                            if (File.Exists(anexo.Caminho))
                            {
                                var novoNome = $"{ anexo.Nome }{ Path.GetRandomFileName()}";
                                anexo.Caminho = $"{caminhoDiretorioAnexos}\\{novoNome}{ anexo.Extensao}";
                                anexo.Nome = novoNome + anexo.Extensao;
                            }
                            else
                            {
                                anexo.Nome = anexo.Nome + anexo.Extensao;
                            }

                            var novoAnexo = new AnexosDeInbox();
                            novoAnexo.Caminho = anexo.Caminho;
                            novoAnexo.Nome = anexo.Nome;
                            novoAnexo.Inbox = inbox;
                            

                            inbox.Anexos.Add(novoAnexo);

                            using (var stream = new FileStream(caminhoFisicoAnexo, FileMode.Create))
                            {
                                stream.Write(anexo.Arquivo);
                            }
                        }
                        await uow.GetRepository<Domain.Entities.Inbox>().UpdateAndSaveAsync(inbox);
                        idsDasMensagens.Add(inbox.Id);
                    }
                }
            }
            else
            {
                throw new ApplicationException("Remetente não encontrado!!!");
            }
            result.Payload = idsDasMensagens;
            return await Task.FromResult(result);
        }
    }
}
