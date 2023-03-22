using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Application.Requests.Inbox.Commands.Excluir;
using Camed.SSC.Application.Requests.Inbox.Commands.MarcarMensagemComoLida;
using Camed.SSC.Application.Requests.Inbox.Commands.Salvar;
using Camed.SSC.Application.Requests.Inbox.Commands.SalvarArquivo;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : RestfullBaseController<Inbox, SalvarInboxCommand, ExcluirInboxCommand>
    {
        IMediator mediator;
        IUnitOfWork uow;

        public InboxController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
            this.mediator = mediator;
            this.uow = uow;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("FormatarArquivo")]
        public async Task<IResult> FormatarArquivo(List<IFormFile> anexos)
        {
            var command = new FormatarArquivoInboxCommand();
            command.FormFiles = anexos;
            return await mediator.Send(command);
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> BaixarArquivo([FromQuery] int idArquivo)
        {
            var caminhoAnexos = await uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS");
            string link = "";
            var anexo = await uow.GetRepository<AnexosDeInbox>().QueryFirstOrDefaultAsync(w => w.Id == idArquivo);
            link = Path.Combine(caminhoAnexos.Valor, anexo.Caminho);
            var fileBytes = System.IO.File.ReadAllBytes(link);
            var extensao = new FileInfo(anexo.Nome).Extension;

            return new FileContentResult(fileBytes, "application/" + extensao)
            {
                FileDownloadName = anexo.Nome
            };
        }

        [HttpPost]
        [Route("marcarMensagemComoLida")]
        public async Task<IActionResult> MarcarMensagemComoLida([FromBody]List<ListById> listById)
        {
            for (int i = 0; i < listById.Count; i++)
            {
                var mensagem = await uow.GetRepository<Domain.Entities.Inbox>().GetByIdAsync(listById[i].Id);

                if (mensagem != null)
                {
                    mensagem.Lida = true;
                    uow.Commit();
                }
                
            }

            return Ok();
        }

        [HttpPut]
        [Route("atualizarLida")]

        public async Task<IResult> AtualizarLida(AtualizarLidaCommand command)
        {
            return await mediator.Send(command);
        }
    }   


}
