using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextoParametrosSistemaController : RestfullBaseController<Domain.Entities.TextoParametrosSistema, SalvarTextoPersonalizadoParametroCommand, ExcluirTextoPersonalizadoParametroCommand >
    {
        public TextoParametrosSistemaController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}