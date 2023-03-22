using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SLAAgendamentoController : ControllerBase
    {
       private readonly IUnitOfWork uow;

       public SLAAgendamentoController(IUnitOfWork uow)
       {
            this.uow = uow;
       }

        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<SLAAgendamento>> Get()
        {
            return await uow.GetRepository<SLAAgendamento>().QueryAsync();
        }

         
    }
}
