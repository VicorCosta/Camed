using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Camed.SSC.WebAPI.Controllers
{
    /// <summary>
    /// Api para gerenciar as ações de acompanhamento
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FrameController : RestfullBaseController<Frame, SalvarFrameCommand, ExcluirFrameCommand>
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="mediator">oriundo do MediatR</param>
        public FrameController(IMediator mediator, IUnitOfWork uow) : base(mediator, uow)
        {

        }
    }
}