using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguradoController : RestfullBaseController<Segurado, SalvarSeguradoCommand, ExcluirSeguradoCommand>
    {
        public SeguradoController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}
