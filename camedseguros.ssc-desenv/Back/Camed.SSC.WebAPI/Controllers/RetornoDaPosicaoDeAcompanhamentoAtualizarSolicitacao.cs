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
    public class RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost]
        public IResult Consultar(int Solicitacao_Id, string DataEHora, int Situacao_Id)
        {
            //if (string.IsNullOrEmpty(Solicitacao_Id) || string.IsNullOrEmpty(Solicitacao_Id))
            //{
            //    throw new ApplicationException("Solicitação não informada");
            //}

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Solicitacao_Id", Solicitacao_Id));
            parameters.Add(new SqlParameter("@DataEHora", DataEHora));
            parameters.Add(new SqlParameter("@Situacao_Id", Situacao_Id));


            var documentos = uow.ExecuteQuery<ResultProcedure>("sp_RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacao @Solicitacao_Id, @DataEHora, @Situacao_Id", parameters.ToArray());

            
            return new Result
            {
                Payload = documentos
            };

        }

        [HttpDelete]
        public IResult DeleteRegistrosAcompanhamento(int Solicitacao_Id, int ordem)
        {
            //if (string.IsNullOrEmpty(Solicitacao_Id) || string.IsNullOrEmpty(Solicitacao_Id))
            //{
            //    throw new ApplicationException("Solicitação não informada");
            //}


            var parametersDel = new List<SqlParameter>();
            parametersDel.Add(new SqlParameter("@ordem", ordem));
            parametersDel.Add(new SqlParameter("@solicitacaoId", Solicitacao_Id));

            var documentos = uow.ExecuteQuery<ResultProcedure>("[dbo].[usp_deleteAcompanhamentoPorOrdem]  @ordem, @solicitacaoId", parametersDel.ToArray());




            return new Result
            {
                Payload = documentos
            };

        }
    }
}