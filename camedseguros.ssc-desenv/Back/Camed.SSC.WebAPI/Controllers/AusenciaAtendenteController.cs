using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AusenciaAtendenteController : RestfullBaseController<Domain.Entities.AusenciaAtendente , SalvarAusenciaAtendenteCommand, ExcluirAusenciaAtendenteCommand>
    {
        public AusenciaAtendenteController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}