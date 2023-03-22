using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Application.Requests.TiposdeParametros.Commands.Excluir;
using Camed.SSC.Application.Requests.TiposdeParametros.Commands.Salvar;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposdeParametrosController : RestfullBaseController<TiposdeParametros, SalvarTipoDeParametroCommand, ExcluirTiposdeParametrosCommand>
    {
        public TiposdeParametrosController (IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}