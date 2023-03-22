using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Domain.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using Camed.SSC.Core;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private RelatorioFiltros relatorioFiltros;

		public RelatorioController(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        /// <summary>
        /// Endpoint no formato OData. Esse endpoint será utilizado para retornar a listagem ou um simples objeto.
        /// </summary>
        /// <returns>Listagem de valores conforme funções do OData que serão aplicados por cada cliente</returns>
        [HttpGet]
        [EnableQuery]
        public object Get(string filtros)
        {
            relatorioFiltros = JsonConvert.DeserializeObject<RelatorioFiltros>(filtros);

            if (relatorioFiltros.TipoRelatorio.Equals("Relatório de Acompanhamentos"))
                return ObterDadosAcompanhamento();
            if (relatorioFiltros.TipoRelatorio.Equals("Monitoria"))
                return ObterDadosMonitoramento();
			if (relatorioFiltros.TipoRelatorio.Equals("Canceladas"))
				return ObterDadosCancelado();
			if (relatorioFiltros.TipoRelatorio.Equals("Avaliação de Atendimento"))
				return ObterDadosAvAtendimento();
			if (relatorioFiltros.TipoRelatorio.Equals("Capa da Proposta de Solicitação"))
				return ObterDaddosPropostaSolicitacao();
			if (relatorioFiltros.TipoRelatorio.Equals("Relatório de Checkin"))
				return ObterDadosRelatorioCheckin();
			if (relatorioFiltros.TipoRelatorio.Equals("Relatório de Conversão"))
                return ObterDadosRelatorioConversao();
			if (relatorioFiltros.TipoRelatorio.Equals("Relatório de Pendência de Documentação (sinistro)"))
				return ObterDadosRelatorioSinistro();
			if (relatorioFiltros.TipoRelatorio.Equals("Agendamentos de Ligação"))
				return ObterDadosAgendamentoLigacao();
			if (relatorioFiltros.TipoRelatorio.Equals("Contabilização Solicitação por Agência"))
				return ObterDadosContAgencia();
			if (relatorioFiltros.TipoRelatorio.Equals("Relatório de Inbox"))
				return ObterDadosRelatorioInbox();
			else
                return null;
        }

		private String ExisteFiltro()
        {
			String result = "";

			if (relatorioFiltros.Solicitacao.Length != 0)
				result += " AND NUMERO=" + relatorioFiltros.Solicitacao;
			if (relatorioFiltros.Segurado.Length != 0)
				result += " AND NOMESEGURADO=" + relatorioFiltros.Segurado;
			if (relatorioFiltros.Superintendencia.Length != 0)
				result += " AND Superintendencia=" + "'" + relatorioFiltros.Superintendencia + "'";
			if (relatorioFiltros.Operador.Length != 0)
				result += " AND OPERADOR=" + "'" + relatorioFiltros.Operador + "'";
			if (relatorioFiltros.SuperConta.Length != 0)
				result += " AND SUPERCONTA=" + "'" + relatorioFiltros.SuperConta + "'";
			if (relatorioFiltros.Atendente.Length != 0)
				result += " AND ATENDENTE=" + "'" + relatorioFiltros.Atendente + "'";
			if (relatorioFiltros.Cnpj.Length != 0)
				result += " AND CPFCNPJ=" + "'" + relatorioFiltros.Cnpj + "'";

            if (relatorioFiltros.DataInicial.Length != 0 && relatorioFiltros.TipoRelatorio != "Relatório de Inbox" && relatorioFiltros.TipoRelatorio != "Canceladas")
                result += " AND DATAHORASITUACAOATUAL>=" + "'" + relatorioFiltros.DataInicial + "'";
            if (relatorioFiltros.DataFinal.Length != 0 && relatorioFiltros.TipoRelatorio != "Relatório de Inbox" && relatorioFiltros.TipoRelatorio != "Canceladas")
                result += " AND DATAHORASITUACAOATUAL<=" + "'" + relatorioFiltros.DataFinal + "'";

			if (relatorioFiltros.DataInicial.Length != 0 && relatorioFiltros.TipoRelatorio == "Relatório de Inbox")
				result += " AND DATACRIACAO>=" + "'" + relatorioFiltros.DataInicial + "'";
			if (relatorioFiltros.DataFinal.Length != 0 && relatorioFiltros.TipoRelatorio == "Relatório de Inbox")
				result += " AND DATACRIACAO<=" + "'" + relatorioFiltros.DataFinal + "'";

			if (relatorioFiltros.DataInicial.Length != 0 && relatorioFiltros.TipoRelatorio == "Canceladas")
				result += " AND DATADEINGRESSO>=" + "'" + relatorioFiltros.DataInicial + "'";
			if (relatorioFiltros.DataFinal.Length != 0 && relatorioFiltros.TipoRelatorio == "Canceladas")
				result += " AND DATADEINGRESSO<=" + "'" + relatorioFiltros.DataFinal + "'";

			if (relatorioFiltros.DataInicialAC.Length != 0)
				result += " AND ACOMPDATA>=" + "'" + relatorioFiltros.DataInicialAC + "'";
			if (relatorioFiltros.DataFinalAC.Length != 0)
				result += " AND ACOMPDATA<=" + "'" + relatorioFiltros.DataFinalAC + "'";

            if (relatorioFiltros.DataFechamentoI.Length != 0)
                result += " AND DataFechamento>=" + "'" + relatorioFiltros.DataFechamentoI + "'";
            if (relatorioFiltros.DataFechamentoF.Length != 0)
                result += " AND DataFechamento<=" + "'" + relatorioFiltros.DataFechamentoF + "'";

            if (relatorioFiltros.AreaNegocio.Length != 0)
				result += " AND AREANEGOCIO=" + "'" + relatorioFiltros.AreaNegocio + "'";
			if (relatorioFiltros.TipoSeguro.Length != 0)
				result += " AND TIPODESEGURO=" + "'" + relatorioFiltros.TipoSeguro + "'";
			if (relatorioFiltros.Segmento.Length != 0)
				result += " AND SEGMENTO=" + "'" + relatorioFiltros.Segmento + "'";
			if (relatorioFiltros.RamoSeguro.Length != 0)
				result += " AND RAMO=" + "'" + relatorioFiltros.RamoSeguro + "'";
			
			if (relatorioFiltros.Status.Length != 0)
            {
				int status = 0;
				if (relatorioFiltros.Status == "Ativo") status = 1;
					
				result += " AND STATUSUSUARIO=" + status;
            }
			
			if (relatorioFiltros.Situacao.Length != 0)
            {
				string[] situacoes = relatorioFiltros.Situacao.Split('|');
				string situacaoResult = FormatStringArgs(situacoes);

				result += " AND SITUACAO IN (" + situacaoResult + ")";
            }

			if (relatorioFiltros.Agencia.Length != 0)
            {
				string[] agencias = relatorioFiltros.Agencia.Split('|');
				string agenciaResult = FormatStringArgs(agencias);

				result += " AND AGENCIA IN (" + agenciaResult + ")";
            }
			if (relatorioFiltros.AgenciaConta.Length != 0)
            {
				string[] agenciaContas = relatorioFiltros.AgenciaConta.Split('|');
				string agenciaContaResult = FormatStringArgs(agenciaContas);
				
				result += " AND AGENCIACONTA IN (" + agenciaContaResult + ")";
			}

			return result;
        }

		private String ExisteFiltroProcedure()
        {
			String result = "";

			if (relatorioFiltros.Solicitacao.Length != 0)
				result += " @Numero=" + relatorioFiltros.Solicitacao + ",";
			if (relatorioFiltros.Agencia.Length != 0)
				result += " @Agencia=" + "'" + relatorioFiltros.Agencia + "',";
			if (relatorioFiltros.Segurado.Length != 0)
				result += " @Segurado=" + "'" + relatorioFiltros.Segurado + "',";
			if (relatorioFiltros.Cnpj.Length != 0)
				result += " @CpfCnpj=" + "'" + relatorioFiltros.Cnpj + "',";
			if (relatorioFiltros.TipoSeguro.Length != 0)
				result += " @TipoDeSeguro=" + "'" + relatorioFiltros.TipoSeguro + "',";
			if (relatorioFiltros.Situacao.Length != 0)
				result += " @SituacaoAtual=" + "'" + relatorioFiltros.Situacao + "',";
			if (relatorioFiltros.AreaNegocio.Length != 0)
				result += " @AreaDeNegocio=" + "'" + relatorioFiltros.AreaNegocio + "',";
			if (relatorioFiltros.Atendente.Length != 0)
				result += " @Atendente=" + "'" + relatorioFiltros.Atendente + "',";
			if (relatorioFiltros.Segmento.Length != 0)
				result += " @Segmento=" + "'" + relatorioFiltros.Segmento + "',";
			
			if (relatorioFiltros.DataInicial.Length != 0)
				result += " @DataDeIngresso=" + "'" + relatorioFiltros.DataInicial + "',";
			if (relatorioFiltros.DataFinal.Length != 0)
				result += " @DataDeIngressoFinal=" + "'" + relatorioFiltros.DataFinal + "',";

			if (relatorioFiltros.UsuarioId.Length != 0)
				result += " @Usuario_Id=" + relatorioFiltros.UsuarioId;


			return result;
		}

		private String FormatStringArgs(string[] items)
        {
			string lastItem = items[items.Length - 2];
			string result = "";

			foreach (var item in items)
			{
				if (item != " ")
				{
					result += "'" + item.Trim() + "'";
					if (item != lastItem) result += ",";
				}
			}
			return result;
		}

		private object ExecuteCommand(String sql)
        {
			List<object> objects = new List<object>();

			using (SqlConnection conn = new SqlConnection(ConnectionString.App))
            {
				conn.Open();

				using (SqlCommand command = new SqlCommand(sql, conn))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							IDictionary<string, object> record = new Dictionary<string, object>();
							for (int i = 0; i < reader.FieldCount; i++)
							{
								record.Add(reader.GetName(i), reader[i]);
							}
							objects.Add(record);
						}
					}
				}
			}

			return objects;
		}

		private object ObterDadosContAgencia()
        {
			String sql = @"EXEC dbo.up_rel_contabilizacao" + ExisteFiltroProcedure();

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosRelatorioInbox()
        {
			String sql1 = @"SELECT '***SUAS MENSAGENS RECEBIDAS***'REMETENTE,''DESTINATARIO,''ASSUNTO,''TEXTO,''LIDA,''DATACRIACAO UNION ALL
							SELECT '','','','','','' UNION ALL
							SELECT ISNULL(U2.NOME,'SISTEMA SSC') REMETENTE,
							U.NOME DESTINATARIO,
							I.ASSUNTO,
							I.TEXTO,
							CASE WHEN LIDA = 1 THEN 'SIM' ELSE 'NÃO' END LIDA,
							CONVERT(VARCHAR, I.DATACRIACAO, 103) DATACRIACAO
							FROM INBOX I
							JOIN USUARIO U ON I.UsuarioDestinatario_Id = U.Id
							LEFT JOIN USUARIO U2 ON I.UsuarioRemetente_Id = U2.Id
							WHERE I.UsuarioDestinatario_Id =" + relatorioFiltros.UsuarioId + ExisteFiltro();

			String sql2 = @"SELECT ''REMETENTE,''DESTINATARIO,''ASSUNTO,''TEXTO,''LIDA,''DATACRIACAO UNION ALL
							SELECT '***SUAS MENSAGENS ENVIADAS***'REMETENTE,''DESTINATARIO,''ASSUNTO,''TEXTO,''LIDA,''DATACRIACAO UNION ALL
							SELECT '','','','','','' UNION ALL
							SELECT ISNULL(U2.NOME,'SISTEMA SSC') REMETENTE,
							U.NOME DESTINATARIO,
							I.ASSUNTO,
							I.TEXTO,
							CASE WHEN LIDA = 1 THEN 'SIM' ELSE 'NÃO' END LIDA,
							CONVERT(VARCHAR, I.DATACRIACAO, 103) DATACRIACAO
							FROM INBOX I
							JOIN USUARIO U ON I.UsuarioDestinatario_Id = U.Id
							LEFT JOIN USUARIO U2 ON I.UsuarioRemetente_Id = U2.Id
							WHERE I.UsuarioRemetente_Id =" + relatorioFiltros.UsuarioId + ExisteFiltro();

			String sqlUnion = sql1 + " UNION ALL " + sql2;

			var objects = ExecuteCommand(sqlUnion);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosAgendamentoLigacao()
        {
			String sql = @"SELECT
							NUMERO,
							CONVERT(VARCHAR, DATAAGENDAMENTO, 103) as DATAAGENDAMENTO,
							LEFT(CONVERT(VARCHAR, DATAAGENDAMENTO, 108),5) as HORAAGENDAMENTO,
							OBJETIVOLIGACAO,
							CONVERT(VARCHAR, DATALIGACAO, 103) as DATALIGACAO,
							LEFT(CONVERT(VARCHAR, DATALIGACAO, 108),5) as HORALIGACAO,
							RETORNOLIGACAO,
							AGENCIA,
							SUPERINTENDENCIA,
							SOLICITANTE,
							AREANEGOCIO,
							TIPODESEGURO,
							RAMO,
							NOMESEGURADO,
							CPFCNPJ,
							ATENDENTE,
							SITUACAO
						FROM VI_AGENDAMENTOSATENDENTE
						WHERE 1 = 1" + ExisteFiltro() + "ORDER BY 1";

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosRelatorioSinistro()
        {
			String sql = @"SELECT
							S.NUMERO, S.DATADEINGRESSO, SE.Nome as 'NOMESEGURADO', AG.NOME AS AGENCIA, AGCONTA.NOME AS AGENCIACONTA, SE.CPFCNPJ,
							TP.Nome as 'RAMO', TD.Nome AS 'NOMEDOCUMENTO'
							FROM Solicitacao S
							LEFT JOIN SolCheckList SC ON S.Id = SC.Solicitacao_Id
							LEFT JOIN Segurado SE ON S.Segurado_Id = SE.Id
							LEFT JOIN TipoDeProduto TP ON S.TipoDeProduto_Id = TP.Id
							LEFT JOIN TipoDeDocumento TD ON SC.TipoDeDocumento_Id = TD.Id
							LEFT JOIN TipoDeSeguro TS ON S.TipoDeSeguro_Id = TS.Id
							LEFT JOIN AGENCIA AG ON S.Agencia_Id = AG.ID
							LEFT JOIN AGENCIA AGCONTA ON S.AgenciaConta_id = AGCONTA.ID
							LEFT JOIN AreaDeNegocio AN ON S.AreaDeNegocio_Id = AN.Id
							LEFT JOIN Usuario UA ON S.Atendente_Id = UA.Id
							LEFT JOIN Usuario UO ON S.Operador_Id = UO.Id
							WHERE CONVERT(DATETIME, S.DataDeIngresso, 103) >= '20181206'
							AND SC.DocumentoAnexado = 1 AND SC.DocumentoAnexadoConfirmado = 0" + ExisteFiltro();

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDaddosPropostaSolicitacao()
        {
			String sql = @"SELECT
							NUMERO,
							CONVERT(VARCHAR, DATADEINGRESSO, 103) DATADEINGRESSO,
							LEFT(CONVERT(VARCHAR, DATADEINGRESSO, 108),5) HORAINGRESSO,
							CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 103) DATASITUACAOATUAL,
							LEFT(CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 108),5) HORASITUACAOATUAL,
							AGENCIA,
							SUPERINTENDENCIA,
							SOLICITANTE,
							AREANEGOCIO,
							TIPODESEGURO,
							SEGMENTO,
							RAMO,
							NOMESEGURADO,
							CPFCNPJ,
							VINCULOCOMBNB,
							CASE WHEN OPERACAODEFINANCIAMENTO = 1 THEN 'SIM' ELSE 'NÃO' END AS OPERACAODEFINANCIAMENTO,
							CONVERT(VARCHAR, DATAFIMVIGENCIA, 103) DATAFIMVIGENCIA,
							DIASPARARENOVACAO,
							TIPOCANCELAMENTO,
							ATENDENTE,
							OPERADOR,
							SITUACAO,
							CASE WHEN STATUSUSUARIO = 1 THEN 'ATIVO'
								 WHEN STATUSUSUARIO = 0 THEN 'INATIVO' ELSE 'SEM ATENDENTE' END AS STATUSUSUARIO,
							CASE WHEN OrcamentoPrevio = 1 THEN 'SIM' ELSE 'NÃO' END AS OrcamentoPrevio,
							CASE WHEN CROSSUP = 1 THEN 'SIM' ELSE 'NÃO' END AS PROJETOCROSSUP,
							CASE WHEN Mercado = 1 THEN 'SIM' ELSE 'NÃO' END as MERCADO,
							NOME_SEGURADORA,
							RAMO_GS,
							TIPO_DE_SEGURO_GS,
							NUMERO_APOLICE_ANT,
							PERC_COMISSAO,
							PERC_AGENCIAMENTO,
							VALOR_DA_IMPORTANCIA_SEGURADO,
							FORMA_PGTO_1A,
							FORMA_PGTO_DEMAIS,
							SEDE_ENVIA_DOC_FISICO,
							NUMERO_VISTORIA,
							CADASTRADOGS,
							CD_ESTUDO,
							VIP,
							RECHACO,
							TIPOCOMISSAORV
						FROM VI_CAPAPROPOSTASOL
						WHERE 1 = 1" + ExisteFiltro() + "ORDER BY 1";

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosRelatorioCheckin()
        {
			String sql = @"SELECT 
							CONVERT (varchar,datahora,103) as DATA,
							convert(varchar,datahora,108) as HORA,
							LONGITUDE,
							LATITUDE,
							LOCALIDADE,
							ENDERECO, 
							NUMERO,
							NOME ATENDENTE 
							FROM checkin c LEFT JOIN Solicitacao s 
								ON s.Id = c.Solicitacao_Id 
							JOIN USUARIO u 
								ON u.id = c.Usuario_Id 
							WHERE 1 = 1" + ExisteFiltro() + "ORDER BY 1";

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosAvAtendimento()
        {
			String sql = @"SELECT
							CONVERT(VARCHAR, AVA.DATAAVALIACAO, 103) DATAAVALIACAO,
							SOL.NUMERO,
							TS.NOME TIPODESEGURO,
							TP.NOME RAMO,
							SE.NOME SEGURADO,
							AG.NOME AGENCIA,
							AG.SUPER SUPERINTENDENCIA,
							AN.NOME AREADENEGOCIO,
							ISNULL(ATE.NOME,'') ATENDENTE,
							OP.NOME OPERADOR,
							AVA.NOTA,
							AVA.OBSERVACAO
						FROM
							AVALIACAODESOLICITACAO AVA
							INNER JOIN SOLICITACAO SOL ON SOL.ID = AVA.SOLICITACAO_ID
							INNER JOIN AGENCIA AG ON SOL.AGENCIA_ID = AG.ID
							INNER JOIN AREADENEGOCIO AN ON SOL.AREADENEGOCIO_ID = AN.ID
							INNER JOIN SITUACAO SIT ON SOL.SITUACAOATUAL_ID = SIT.ID
							INNER JOIN TIPODEPRODUTO TP ON SOL.TIPODEPRODUTO_ID = TP.ID
							INNER JOIN TIPODESEGURO TS ON SOL.TIPODESEGURO_ID = TS.ID
							INNER JOIN SEGURADO SE ON SOL.SEGURADO_ID = SE.ID
							LEFT  JOIN USUARIO ATE ON SOL.ATENDENTE_ID = ATE.ID
							INNER JOIN USUARIO OP ON OP.Id = SOL.OPERADOR_ID
							LEFT JOIN SEGMENTO SEG ON SOL.SEGMENTO_ID = SEG.ID
						WHERE 1 = 1" + ExisteFiltro() + "ORDER BY 1";
			
			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosCancelado()
        {
			String sql = @"SELECT
							NUMERO,
							OBSERVACAO
						 FROM VI_CANCELADOS
						 WHERE 1 = 1" + ExisteFiltro() + "ORDER BY 1";
			
			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

		private object ObterDadosAcompanhamento()
        {
			String sql = @"SELECT
							NUMERO,
							CONVERT(VARCHAR, DATADEINGRESSO, 103) DATADEINGRESSO,
							LEFT(CONVERT(VARCHAR, DATADEINGRESSO, 108),5) HORAINGRESSO,
							CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 103) DATASITUACAOATUAL,
							LEFT(CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 108),5) HORASITUACAOATUAL,
							AGENCIA,
							SUPERINTENDENCIA,
							SOLICITANTE,
							AREANEGOCIO,
							TIPODESEGURO,
							SEGMENTO,
							RAMO,
							NOMESEGURADO,
							CPFCNPJ,
							VINCULOCOMBNB,
							CASE WHEN OPERACAODEFINANCIAMENTO = 1 THEN 'SIM' ELSE 'NÃO' END AS OPERACAODEFINANCIAMENTO,
							CONVERT(VARCHAR, DATAFIMVIGENCIA, 103) DATAFIMVIGENCIA,
							TIPOCANCELAMENTO,
							DIASPARARENOVACAO,
							ATENDENTE,
							OPERADOR,
							SITUACAO,
							CONVERT(VARCHAR, ACOMPDATA, 103) ACOMPDATAFORMATADA,
							ACOMPDATA,
							ACOMSITUACAO,
                            MOTIVODARECUSA,
							OBSERVACAO,
							ACOMATENDENTE,
							GRUPO,
							ACOMPSLADEF,
							ACOMPSLAEFET,
							ACOMPSLAACUM,
							CASE WHEN STATUSUSUARIO = 1 THEN 'ATIVO'
								 WHEN STATUSUSUARIO = 0 THEN 'INATIVO' ELSE 'SEM ATENDENTE' END AS STATUSUSUARIO,
							CASE WHEN OrcamentoPrevio = 1 THEN 'SIM' ELSE 'NÃO' END AS OrcamentoPrevio,
							CASE WHEN CROSSUP = 1 THEN 'SIM' ELSE 'NÃO' END AS PROJETOCROSSUP,
							CASE WHEN Mercado = 1 THEN 'SIM' ELSE 'NÃO' END as MERCADO,
							AGENCIACONTA,
							SUPERCONTA,
                            SEGURADORA,
                            ESTUDO_ORIGEM,
                            ACOMPATENDENTEREAL
					    FROM VI_ACOMPANHAMENTO
					    WHERE 1 = 1 " + ExisteFiltro() + " ORDER BY NUMERO, ACOMPDATA";

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);
			
			return jsonResult;
		}

        private object ObterDadosMonitoramento()
        {
            String sql = @"SELECT
								NUMERO,
								CONVERT(VARCHAR, DATADEINGRESSO, 103) DATADEINGRESSO,
								LEFT(CONVERT(VARCHAR, DATADEINGRESSO, 108),5) HORAINGRESSO,
								CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 103) DATASITUACAOATUAL,
								LEFT(CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 108),5) HORASITUACAOATUAL,
								AGENCIA,
								SUPERINTENDENCIA,
								SOLICITANTE,
								AREANEGOCIO,
								TIPODESEGURO,
								SEGMENTO,
								RAMO,
								NOMESEGURADO,
								CPFCNPJ,
								VINCULOCOMBNB,
								CASE WHEN OPERACAODEFINANCIAMENTO = 1 THEN 'SIM' ELSE 'NÃO' END AS OPERACAODEFINANCIAMENTO,
								CONVERT(VARCHAR, DATAFIMVIGENCIA, 103) DATAFIMVIGENCIA,
								TIPOCANCELAMENTO,
								DIASPARARENOVACAO,
								ATENDENTE,
								OPERADOR,
								SITUACAO,
								CASE WHEN STATUSUSUARIO = 1 THEN 'ATIVO'
									 WHEN STATUSUSUARIO = 0 THEN 'INATIVO' ELSE 'SEM ATENDENTE' END AS STATUSUSUARIO,
								CASE WHEN OrcamentoPrevio = 1 THEN 'SIM' ELSE 'NÃO' END AS OrcamentoPrevio,
								CASE WHEN CROSSUP = 1 THEN 'SIM' ELSE 'NÃO' END AS PROJETOCROSSUP,
								CASE WHEN Mercado = 1 THEN 'SIM' ELSE 'NÃO' END as MERCADO,
								PERC_ANTERIOR,
								VLRPREMIOLIQANT,
								PERC_ATUAL,
								VLPREMIOLIQATUAL,
                                PERC_PROPOSTA,
                                VLPREMIOLIQPROPOSTA,
								NUMNOVO,
								AGENCIACONTA,
								SUPERCONTA,
                                SEGURADORA
							FROM VI_MONITORIA
							WHERE 1 = 1 " + ExisteFiltro() + " ORDER BY 1";

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}

        private object ObterDadosRelatorioConversao()
        {
			String sql = @"SELECT
								NUMERO,
								CONVERT(VARCHAR, DATADEINGRESSO, 103) DATADEINGRESSO,
								LEFT(CONVERT(VARCHAR, DATADEINGRESSO, 108),5) HORAINGRESSO,
								CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 103) DATASITUACAOATUAL,
								LEFT(CONVERT(VARCHAR, DATAHORASITUACAOATUAL, 108),5) HORASITUACAOATUAL,
								AGENCIA,
								SUPERINTENDENCIA,
								SOLICITANTE,
								AREANEGOCIO,
								TIPODESEGURO,
								SEGMENTO,
								RAMO,
								NOMESEGURADO,
								CPFCNPJ,
								VINCULOCOMBNB,
								ATENDENTE,
								SITUACAO,
								CASE WHEN OPERACAODEFINANCIAMENTO = 1 THEN 'SIM' ELSE 'NÃO' END AS OPERACAODEFINANCIAMENTO,
								CONVERT(VARCHAR, DATAFIMVIGENCIA, 103) DATAFIMVIGENCIA,
								DIASPARARENOVACAO,
								TIPOCANCELAMENTO,
								CASE WHEN STATUSUSUARIO = 1 THEN 'ATIVO'
									 WHEN STATUSUSUARIO = 0 THEN 'INATIVO' ELSE 'SEM ATENDENTE' END AS STATUSUSUARIO,
								TEMPO_DECORRIDO,
								CONVERT(VARCHAR, DATAHORACOTACAO, 103) DATACOTACAO,
								LEFT(CONVERT(VARCHAR, DATAHORACOTACAO, 108),5) HORACOTACAO,
								SITUACAOCOTACAO,
								CONVERT(VARCHAR(10), DATAFECHAMENTO, 103) DATAFECHAMENTO,
								SITUACAOFECHAMENTO,
								MESANOINGRESSO,
								MESANOCOTACAO,
								TEMPOTOTAL,
								MESANOFECHAMENTO,
								VLR_PL_ANTERIOR,
								PERC_COM_ANTERIOR,
								VLR_PL_COTACAO,
								PERC_COM_COTACAO,
								SGD_COTACAO,
								VLR_PL_FINAL,
								PERC_COM_FINAL,
                                PERC_CO_CORRETAGEM,
								SGD_FINAL,
                                [ESTUDO/PROPOSTA],
                                VENDA_COMPARTILHADA,
                                NUMERO_PROPOSTA_SEGURADORA,
                                VL_IMPORTANCIA_SEGURADA,
                                MERCADO,
                                ESTUDO_ORIGEM
							FROM VI_DADOSCONVERSAO
							WHERE 1 = 1 " + ExisteFiltro() + " ORDER BY 1";

			var objects = ExecuteCommand(sql);
			string jsonResult = JsonConvert.SerializeObject(objects);

			return jsonResult;
		}
    }
}