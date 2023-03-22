using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Application.Util;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteConexaoController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public TesteConexaoController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] string email)
        {
            new MailClient(uow).SendMail(new[] { email }, "Teste de conexão", "Esse é um e-mail de teste de conexão. Não responder.");
            return Ok();
        }
    }
}