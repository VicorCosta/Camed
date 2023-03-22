using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosSistemaController : RestfullBaseController<Domain.Entities.ParametrosSistema, SalvarParametroSistemaCommand, ExcluirParametroSistemaCommand>
    {
        public ParametrosSistemaController (IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}