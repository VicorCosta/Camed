using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMorteController : RestfullBaseController<TipoMorte, SalvarTipoMorteCommand, ExcluirTipoMorteCommand>
    {
        public TipoMorteController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}