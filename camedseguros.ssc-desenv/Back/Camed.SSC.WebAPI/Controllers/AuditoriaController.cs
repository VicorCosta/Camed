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
    public class AuditoriaController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public AuditoriaController(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        /// <summary>
        /// Endpoint no formato OData. Esse endpoint será utilizado para retornar a listagem ou um simples objeto.
        /// </summary>
        /// <returns>Listagem de valores conforme funções do OData que serão aplicados por cada cliente</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<List<AuditoriaFiltroResult>> Get([FromQuery]AuditoriaFiltroDTO filter)
        {
            var parameters = new Dictionary<string, SqlParameter>();
            parameters.Add("@datainicial = @di", new SqlParameter("@di", filter.DataInicial));
            parameters.Add("@dataFinal = @df", new SqlParameter("@df", filter.DataFinal));

            if (!string.IsNullOrEmpty(filter.Usuario))
            {
                parameters.Add("@usuario = @u", new SqlParameter("@u", filter.Usuario));
            }

            if (filter.Evento.HasValue)
            {
                parameters.Add("@evento = @e", new SqlParameter("@e", filter.Evento));
            }

            if (!string.IsNullOrEmpty(filter.Tabela))
            {
                parameters.Add("@tabela = @t", (new SqlParameter("@t", filter.Tabela)));
            }

            if (filter.Chave.HasValue)
            {
                parameters.Add("@chave = @c", new SqlParameter("@c", filter.Chave));
            }

            if (filter.NumeroSolicitacao.HasValue)
            {
                parameters.Add("@numerosolicitacao = @ns", new SqlParameter("@ns", filter.NumeroSolicitacao));
            }

            if (!string.IsNullOrEmpty(filter.Mensagem))
            {
                parameters.Add("@mensagem = @m", new SqlParameter("@m", filter.Mensagem));
            }


            var query = $"exec [dbo].[usp_AuditLog] {string.Join(",", parameters.Keys)}";

            var data = await uow.ExecuteQuery<AuditoriaFiltroResult>(query, parameters.Values.Cast<SqlParameter>().ToArray()).ToListAsync();

            return data;
        }
    }
}   