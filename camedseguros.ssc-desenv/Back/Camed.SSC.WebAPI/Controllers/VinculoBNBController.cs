using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinculoBNBController : RestfullBaseController<VinculoBNB, SalvarVinculoBNBCommand, ExcluirVinculoBNBCommand>
    {
        public VinculoBNBController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}