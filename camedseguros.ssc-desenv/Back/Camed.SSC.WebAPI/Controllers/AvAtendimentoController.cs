using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Application.Requests.AvaliacaoAtendimento.Commands.Excluir;
using Camed.SSC.Application.Requests.AvaliacaoAtendimento.Commands.Salvar;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvAtendimentoController : RestfullBaseController<AvAtendimento, SalvarAvAtendimentoCommand, ExcluirAvAtendimentoCommand>
    {
        public AvAtendimentoController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}
