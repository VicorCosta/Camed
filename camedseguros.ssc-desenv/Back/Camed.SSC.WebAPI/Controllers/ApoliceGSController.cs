using Camed.SCC.Infrastructure.CrossCutting.Dto;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core;
using Camed.SSC.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    /// <summary>
    /// Controller responsável por obter os dados do menu Consultar Documento Apólice
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApoliceGSController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public ApoliceGSController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        /// <summary>
        /// Consulta os dados de apólices
        /// </summary>
        /// <param name="agencia"></param>
        /// <param name="cpfcgc"></param>
        /// <param name="tipoSolicitacao"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Consultar")]
        public IResult Consultar(string agencia, string cpfcgc, string tipoSolicitacao)
        {
            if (string.IsNullOrEmpty(agencia) )
            {
                throw new ApplicationException("Agência e/ou CPF/CGC não informado.");
            }

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@agencia", agencia));
            parameters.Add(new SqlParameter("@cpfcgc", cpfcgc));
            parameters.Add(new SqlParameter("@tipo", tipoSolicitacao));

            var documentos = uow.ExecuteQuery<ApoliceGsDTO>("ConsultaApolicesGSVersao2 @agencia, @cpfcgc, @tipo", parameters.ToArray());

            return new Result
            {
                Payload = documentos
            };
        }

        /// <summary>
        /// Obtém o documento de apólice informado
        /// </summary>
        /// <param name="caminho"></param>
        /// <returns></returns>
        [HttpGet]
        
        public async Task<IActionResult> ObterArquivo(string caminho)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(caminho, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(caminho));
        }
    }
}