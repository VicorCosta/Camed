using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests.Solicitante.Command.Excluir;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitanteController : RestfullBaseController<Solicitante, SalvarSolicitanteCommand, ExcluirSolicitanteCommand>
    {
        public SolicitanteController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}
