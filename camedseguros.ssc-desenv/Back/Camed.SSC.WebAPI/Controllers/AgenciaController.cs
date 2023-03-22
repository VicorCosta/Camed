using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Domain.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciaController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public AgenciaController(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        /// <summary>
        /// Endpoint no formato OData. Esse endpoint será utilizado para retornar a listagem ou um simples objeto.
        /// </summary>
        /// <returns>Listagem de valores conforme funções do OData que serão aplicados por cada cliente</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<Agencia>> Get()
        {
            return await uow.GetRepository<Agencia>().QueryAsync();
        }
    }
}