using Camed.SSC.Application.Requests;
using Camed.SSC.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    /// <summary>
    /// Api para autenticação do sistema SSC
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator mediator;

        public LoginController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Endpoint onde se realiza o login para acessar o sistema SSC
        /// </summary>
        /// <param name="command">Objeto com os atributos "Login" e "Senha" a serem verificados para o acesso ao sistema SSC</param>
        /// <returns>Token de acesso para ser utilizado no front-end</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IResult> Post([FromBody] LoginCommand command)
        {
            return await mediator.Send(command);
        }
    }
}