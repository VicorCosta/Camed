using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapeamentoAtendenteController : RestfullBaseController<Domain.Entities.MapeamentoAtendente , SalvarMapeamentoDeAtendenteCommand, ExcluirMapeamentoDeAtendenteCommand>
    {
        public MapeamentoAtendenteController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}