using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    /// <summary>
    /// Api para gerenciar as situaçãoes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("Bearer")]
    public class SituacaoController : RestfullBaseController<Situacao, SalvarSituacaoCommand, ExcluirSituacaoCommand>
    {
        public SituacaoController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {

        }
    }
}