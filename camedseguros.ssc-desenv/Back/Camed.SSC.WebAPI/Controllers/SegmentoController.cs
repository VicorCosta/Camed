using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests.Segmento.Commands.Excluir;
using Camed.SSC.Application.Requests.Segmento.Commands.Salvar;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentoController : RestfullBaseController<Segmento, SalvarSegmentoCommand, ExcluirSegmentoCommand>
    {
        public SegmentoController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {
        }
    }
}
