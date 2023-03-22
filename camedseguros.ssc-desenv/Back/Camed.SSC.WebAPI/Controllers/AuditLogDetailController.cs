using Camed.SCC.Infrastructure.CrossCutting.Dto;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Domain.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogDetailController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public AuditLogDetailController(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        /// <summary>
        /// Endpoint no formato OData. Esse endpoint será utilizado para retornar a listagem ou um simples objeto.
        /// </summary>
        /// <returns>Listagem de valores conforme funções do OData que serão aplicados por cada cliente</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<List<AuditoriaDetailsFiltroResult>> Get([FromQuery]AuditoriaDetailsFiltroDTO filter)
        {
            var parameters = new Dictionary<string, SqlParameter>();
            parameters.Add("@RecordId = @r", new SqlParameter("@r", filter.RecordId));
            parameters.Add("@datainicial = @di", new SqlParameter("@di", filter.DataInicial));
            parameters.Add("@dataFinal = @df", new SqlParameter("@df", filter.DataFinal));


           /* if (!string.IsNullOrEmpty(filter.ColumnName))
            {
                parameters.Add("@columnname = @col", new SqlParameter("@col", filter.ColumnName));
            }

            if (!string.IsNullOrEmpty(filter.OriginalValue))
            {
                parameters.Add("@originalvalue = @ori", new SqlParameter("@ori", filter.OriginalValue));
            }

            if (!string.IsNullOrEmpty(filter.NewValue))
            {
                parameters.Add("@newvalue = @newv", new SqlParameter("@newv", filter.NewValue));
            }*/

            var query = $"exec [dbo].[usp_AuditLogDetailSelect] {string.Join(",", parameters.Keys)}";

            var data = await uow.ExecuteQuery<AuditoriaDetailsFiltroResult>(query, parameters.Values.Cast<SqlParameter>().ToArray()).ToListAsync();

            return data;
        }
    }
}   