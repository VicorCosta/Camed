using AutoMapper.Configuration;
using Camed.SCC.Infrastructure.CrossCutting.Dto;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Util;
using Camed.SSC.Core;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using FluentValidation.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaApolice
{
    public class ConsultaApoliceCommandHandler : HandlerBase, IRequestHandler<ConsultaApoliceCommand, IResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public ConsultaApoliceCommandHandler(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
        }

        public async Task<IResult> Handle(ConsultaApoliceCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var query = String.Empty;
                var param_apolice = String.Empty;
                var param_cpf_cnpj = String.Empty;
                var param_apolice_gs = String.Empty;
                var param_cpf_cnpj_gs = String.Empty;

                string propostaQuiver = String.Empty;
                string nomeSeguradora = String.Empty;
                string endosso = String.Empty;
                string dtInicioVigencia = String.Empty;
                string link = String.Empty;
                string erro = String.Empty;
                string msg = String.Empty;

                string instrucao1 = String.Empty;
                string fone = String.Empty;
                string email = String.Empty;
                string url_seguradora = String.Empty;

                string codigoDoBem = String.Empty;
                string produto = String.Empty;

                string tipo_arquivo = String.Empty;

                if (!String.IsNullOrEmpty(request.apolice) || !String.IsNullOrEmpty(request.cpf_cnpj))
                {


                    if (!String.IsNullOrEmpty(request.apolice))
                    {
                        param_apolice = "d.Apolice = '" + request.apolice + "'";
                        param_apolice_gs = "CAST(s.cd_apolice AS varchar(20)) = '" + request.apolice + "'";
                    }
                    else
                    {
                        param_apolice = "1=1";
                        param_apolice_gs = "1=1";
                    }
                    if (!String.IsNullOrEmpty(request.cpf_cnpj))
                    {
                        if (Validacoes.ValidaCPFCNPJ(request.cpf_cnpj))
                        {
                            request.cpf_cnpj = FormatCnpjCpf.SemFormatacao(request.cpf_cnpj);
                            if (request.cpf_cnpj.Length == 11)
                            {
                                request.cpf_cnpj = FormatCnpjCpf.FormatCPF(request.cpf_cnpj);
                            }
                            else
                            {
                                request.cpf_cnpj = FormatCnpjCpf.FormatCNPJQuiver(request.cpf_cnpj);
                            }
                            param_cpf_cnpj = "c.cgc_cpf = '" + request.cpf_cnpj + "'";
                            param_cpf_cnpj_gs = "CAST(sgd.nu_cpf_cgc AS varchar(15)) = CAST(CAST('" + request.cpf_cnpj.Replace(".", "").Replace("/", "").Replace("-", "") + "' AS decimal) AS varchar(15))";
                        }
                        else
                        {
                            throw new ApplicationException("CPF/CNPJ inválido.");
                        }
                    }
                    else
                    {
                        param_cpf_cnpj = "1=1";
                        param_cpf_cnpj_gs = "1=1";
                    }
                }
                else
                {
                    throw new ApplicationException("Parâmetros Apólice ou CPF/CNPJ não informado.");
                }

                List<ApoliceBoletoDTO> list = new List<ApoliceBoletoDTO>();

                tipo_arquivo = "Apolice.pdf";
                #region  --- Query SELECT * FROM Listagem --- 
                query = String.Format(@"
                            ;WITH Listagem 
                            AS
                            (
                                SELECT d.Proposta AS PropostaQuiver
                                , c.cgc_cpf AS Cpf_Cnpj
                                , d.Apolice
                                , d.Documento
                                , d.Alteracao
                                , CASE WHEN d.endosso = '' THEN 'Apolice mãe' ELSE d.endosso END AS Endosso
								, CONVERT(VARCHAR(10), d.inicio_vigencia, 103) AS InicioVigencia
                                , d.Proposta_cia
                                , d.Seguradora AS CodSeguradora
                                , ins.Seguradora
                                , ins.Url
                                , ins.Instrucao1
                                , ins.Fone
                                , ins.E_Mail
                                , img.Path COLLATE Latin1_General_CI_AS AS Imagem
                                , '1' AS Origem
                                FROM CCSW1001V.quiver.dbo.Tabela_Documentos AS d WITH (NOLOCK) 
                                INNER JOIN CCSW1001V.quiver.dbo.tabela_clientes AS c WITH (NOLOCK) on c.cliente = d.cliente        
                                INNER JOIN CCSW1001V.quiver.dbo.Tabela_Seguradoras AS s WITH (NOLOCK) on s.Seguradora = d.Seguradora
                                JOIN workflow.dbo.DeParaSeguradoraGsQuiver gsq ON gsq.CodSeguradoraQuiver = d.Seguradora
                                LEFT JOIN workflow.dbo.Tb_Seguradoras_Instrucao_Quiver AS ins WITH (NOLOCK) ON ins.CodSeguradora = gsq.CodSeguradoraQuiver 
                                LEFT  JOIN CCSW1001V.quiver.dbo.TABELA_SCANIMAGENS AS IMG WITH (NOLOCK) on (d.DOCUMENTO = IMG.DOCUMENTO        
                                            AND d.ALTERACAO = IMG.ALTERACAO
                                            AND d.CLIENTE = IMG.CLIENTE
                                            AND (img.descricao like '%APOLICE%' or img.descricao like '%APÓLICE%' or img.Cod_tipoimg = 4) ) 
                                WHERE d.Tipo_Negocio <> 0 -- 4
                                    AND ( {0} AND {1} )
                                UNION ALL
                                SELECT s.cd_estudo AS PropostaQuiver
                                , dbo.formatarCNPJCPFQuiver(CAST(sgd.nu_cpf_cgc AS varchar(15))) AS Cpf_Cnpj
                                , CAST(s.cd_apolice AS varchar(20)) AS Apolice
                                , CAST(s.NU_DOC_ENDOSSO AS varchar(20)) AS Documento
                                , null AS Alteracao
                                , '' AS Endosso
								, '' AS InicioVigencia
                                , null AS Proposta_cia
                                , s.cd_seguradora AS CodSeguradora
                                , ins.Seguradora
                                , ins.Url
                                , ins.Instrucao1
                                , ins.Fone
                                , ins.E_Mail
                                , tppa.nm_path +'\'+tppa.nm_file AS Imagem
                                , '2' AS Origem
                                FROM [CCSW0101V].[GSCAMED].dbo.tb_seguro s WITH (NOLOCK)
                                JOIN [CCSW0101V].[GSCAMED].dbo.tb_segurado sgd WITH (NOLOCK) ON sgd.cd_pessoa_principal = s.cd_pessoa_principal 
                                    AND sgd.cd_segurado_principal = s.cd_segurado_principal 
                                JOIN [CCSW0101V].[GSCAMED].dbo.tb_seguradora sgda WITH (NOLOCK) ON sgda.cd_pessoa_seguradora = s.cd_pessoa_seguradora
	                                AND sgda.cd_seguradora = s.cd_seguradora 
                                JOIN workflow.dbo.DeParaSeguradoraGsQuiver gsq WITH (NOLOCK) ON gsq.CodSeguradoraGS = s.cd_seguradora 
                                LEFT JOIN workflow.dbo.Tb_Seguradoras_Instrucao_Quiver AS ins WITH (NOLOCK) ON ins.CodSeguradora = gsq.CodSeguradoraQuiver
                                JOIN [CCSW0101V].[GSCAMED].dbo.tb_pdf_proposta_apolice tppa WITH (NOLOCK) ON tppa.cd_apolice = s.cd_apolice
														AND tppa.cd_ramo = s.cd_ramo
														AND tppa.cd_sub_ramo = s.cd_sub_ramo
														AND tppa.cd_seguradora = s.cd_seguradora
                                WHERE s.fl_situacao = 4
                                    AND ( {2} AND {3} )
                            )
                            SELECT *
                            FROM Listagem
                            ", param_apolice, param_cpf_cnpj, param_apolice_gs, param_cpf_cnpj_gs);

                #endregion

                query = String.Format(@"EXEC dbo.ListaDeApolicesParaTallos '{0}' , '{1}' ", request.apolice, request.cpf_cnpj);

                using (var connection = new SqlConnection(ConnectionString.App))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var dt = new DataTable();
                            dt.Load(reader);

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {

                                var codSeguradora = dt.Rows[i][8].ToString();

                                instrucao1 = dt.Rows[i][11].ToString();
                                fone = dt.Rows[i][12].ToString();
                                email = dt.Rows[i][13].ToString();
                                produto = dt.Rows[i][16].ToString();
                                codigoDoBem = dt.Rows[i][17].ToString();

                                string nomeDoArquivo = String.Empty;
                                string caminhoParaDoArquivo = String.Empty;

                                if (dt.Rows[i][15].ToString() == "1")
                                {
                                    nomeDoArquivo = dt.Rows[i][14].ToString();
                                    nomeDoArquivo = nomeDoArquivo.Replace("e:\\Imagens\\", "");
                                    caminhoParaDoArquivo = @"\\ccsw1010vp\imagens\" + nomeDoArquivo;
                                }
                                else
                                {
                                    nomeDoArquivo = dt.Rows[i][14].ToString();
                                    nomeDoArquivo = nomeDoArquivo.Replace("e:\\Imagens\\", "");
                                    caminhoParaDoArquivo = nomeDoArquivo;
                                }

                                byte[] AsBytes = { 0 };
                                String AsBase64String = "Sem apólice disponivel para baixar, entre em contato com a seguradora.";

                                if (string.IsNullOrEmpty(caminhoParaDoArquivo) || !File.Exists(caminhoParaDoArquivo))
                                {
                                    link = "";
                                    msg = String.Format(instrucao1, request.tipo.ToUpper());
                                    erro = "409";
                                }
                                else
                                {
                                    AsBytes = ASCIIEncoding.ASCII.GetBytes(tipo_arquivo + ":" + caminhoParaDoArquivo);
                                    AsBase64String = Convert.ToBase64String(AsBytes);

                                    link = request.Link;
                                    link = link.Replace("/api/solicitacao/consultaApoliceBoleto", "/api/solicitacao/downloadApoliceBoleto/");

                                    msg = "Apólice encontrada.";
                                    erro = "";
                                }

                                list.Add(new ApoliceBoletoDTO()
                                {
                                    PropostaQuiver = dt.Rows[i][0].ToString(),
                                    Cpf_Cnpj = dt.Rows[i][1].ToString(),
                                    Apolice = dt.Rows[i][2].ToString(),
                                    Documento = dt.Rows[i][3].ToString(),
                                    Alteracao = dt.Rows[i][4].ToString(),
                                    Endosso = dt.Rows[i][5].ToString(),
                                    InicioVigencia = dt.Rows[i][6].ToString(),
                                    Proposta_cia = dt.Rows[i][7].ToString(),
                                    CodSeguradora = codSeguradora,
                                    Seguradora = dt.Rows[i][9].ToString(),
                                    Link = link + AsBase64String,
                                    Tipo = "",
                                    Url = dt.Rows[i][10].ToString(),
                                    Instrucao1 = dt.Rows[i][11].ToString(),
                                    Fone = dt.Rows[i][12].ToString(),
                                    E_mail = dt.Rows[i][13].ToString(),
                                    Produto = dt.Rows[i][16].ToString(),
                                    CodigoDoBem = dt.Rows[i][17].ToString(),
                                    Erro = erro,
                                    Msg = msg
                                });
                            }
                        }
                    }
                }

                string lista = String.Empty;

                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        propostaQuiver = item.PropostaQuiver;
                        request.apolice = item.Apolice;
                        endosso = item.Endosso;
                        dtInicioVigencia = item.InicioVigencia;
                        nomeSeguradora = item.Seguradora;
                        link = item.Link;
                        instrucao1 = item.Instrucao1;
                        fone = item.Fone;
                        email = item.E_mail;
                        url_seguradora = item.Url;
                        produto = item.Produto;
                        codigoDoBem = item.CodigoDoBem;
                        erro = item.Erro;
                        msg = item.Msg;
                        lista += "{\"Proposta\":\"" + propostaQuiver + "\", \"Apolice\":\"" + request.apolice + "\", \"Endosso\":\"" + endosso + "\", \"Data Inicio Vigencia\":\"" + dtInicioVigencia + "\", \"Seguradora\":\"" + nomeSeguradora + "\", \"Tipo\":\"" + request.tipo + "\", \"Produto\":\"" + produto + "\", \"CodigoDoBem\":\"" + codigoDoBem + "\", \"Erro\":\"" + erro + "\", \"Msg\":\"" + msg + "\", \"Link\":\"" + link + "\", \"SeguradoraSite\":\"" + url_seguradora + "\", \"SeguradoraTelefone\":\"" + fone + "\", \"SeguradoraEmail\":\"" + email + "\"}";
                        lista += ",";
                    }
                }
                else
                {
                    msg = "Não foi encontrado nenhum registro com esse dados informado.";
                    erro = "409";
                    lista += "{\"Proposta\":\"" + propostaQuiver + "\", \"Apolice\":\"" + request.apolice + "\", \"Endosso\":\"" + endosso + "\", \"Data Inicio Vigencia\":\"" + dtInicioVigencia + "\", \"Seguradora\":\"" + nomeSeguradora + "\", \"Tipo\":\"" + request.tipo + "\", \"Produto\":\"" + produto + "\", \"CodigoDoBem\":\"" + codigoDoBem + "\", \"Erro\":\"" + erro + "\", \"Msg\":\"" + msg + "\", \"Link\":\"" + link + "\", \"SeguradoraSite\":\"" + url_seguradora + "\", \"SeguradoraTelefone\":\"" + fone + "\", \"SeguradoraEmail\":\"" + email + "\"}";
                }

                lista += ",";

                if (erro == "409" && list.Count >= 0 && list.Count <= 1)
                {
                    throw new ApplicationException(msg);
                }

                return await Task.FromResult(result);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
