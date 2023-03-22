using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapeamentoAcaoSituacaoController : RestfullBaseController<MapeamentoAcaoSituacao, SalvarFluxoDeOperacaoCommand, ExcluirFluxoDeOperacaoCommand>
    {
        public MapeamentoAcaoSituacaoController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}