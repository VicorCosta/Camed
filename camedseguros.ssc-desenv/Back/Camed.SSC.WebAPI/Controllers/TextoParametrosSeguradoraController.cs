using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextoParametroSeguradoraController : RestfullBaseController<Domain.Entities.TextoParametroSeguradora, SalvarTextoParametroSeguradoraCommand, ExcluirTextoParametroSeguradoraCommand >
    {
        public TextoParametroSeguradoraController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}