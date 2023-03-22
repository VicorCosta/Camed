using Camed.SCC.Infrastructure.CrossCutting.Dto;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Util;
using Camed.SSC.Core;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaSinistro
{
    public class ConsultaSinistroCommandHandler : HandlerBase, IRequestHandler<ConsultaSinistroCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public ConsultaSinistroCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(ConsultaSinistroCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var query = String.Empty;
                var param_apolice = String.Empty;
                var param_cpf_cnpj = String.Empty;

                string sApolice = String.Empty;
                string sDocumento = String.Empty;
                string sAlteracao = String.Empty;
                string sProposta_cia = String.Empty;
                string sCliente = String.Empty;
                string sCgc_cpf = String.Empty;
                string sCodSeguradora = String.Empty;
                string sSeguradora = String.Empty;
                string sHome_page = String.Empty;
                string sTelefone = String.Empty;
                string sDDD = String.Empty;
                string sE_mail = String.Empty;
                string sSegurado = String.Empty;
                string sNumeroAviso = String.Empty;
                string sNumeroSinistroCia = String.Empty;
                string sNumeroApolice = String.Empty;
                string sNumeroPropostaQuiver = String.Empty;
                string sDataDoSinistro = String.Empty;
                string sProduto = String.Empty;
                string sCodigo = String.Empty;
                string sStatus = String.Empty;
                string erro = String.Empty;
                string msg = String.Empty;

                string instrucao2 = String.Empty;
                string urlContato = String.Empty;
                string foneContato = String.Empty;
                string emailContato = String.Empty;

                if (!String.IsNullOrEmpty(request.apolice) || !String.IsNullOrEmpty(request.cpf_cnpj))
                {
                    if (!String.IsNullOrEmpty(request.apolice))
                    {
                        param_apolice = "d.Apolice = '" + request.apolice + "'";
                    }
                    else
                    {
                        param_apolice = "1=1";
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
                        }
                        else
                        {
                            throw new ApplicationException("CPF/CNPJ inválido.");
                        }
                    }
                    else
                    {
                        param_cpf_cnpj = "1=1";
                    }
                }
                else
                {
                    throw new ApplicationException("Parâmetros Apólice ou CPF/CNPJ não informado");
                }

                List<ApoliceSinistroDTO> list = new List<ApoliceSinistroDTO>();

                #region -- Query lista
                query = String.Format(@"
                        SELECT TOP 1 
	                         d.Apolice
	                        ,d.Documento, d.Alteracao, d.Proposta_cia
	                        ,c.Cliente, c.Cgc_cpf
                        -- -----------------------	
	                        ,d.Seguradora AS CodSeguradora, s.Nome AS Seguradora
	                        ,s.Home_page	  
                            ,ISNULL(cf.Telefone, ins.Fone) AS Telefone
                            ,cf.DDD
	                        ,ce.E_mail
                            ,ins.Instrucao2,ins.Url,ins.Fone,ins.E_Mail
                        -- -----------------------
	                        ,c.Nome AS Segurado
	                        ,sini.Aviso AS NumeroAviso
	                        ,sini.Sinistro_cia AS NumeroSinistroCia
	                        ,d.Apolice AS NumeroApolice
	                        ,d.Proposta AS NumeroPropostaQuiver
	                        ,CONVERT(varchar(10), sini.Data_sinistro,103) AS DataDoSinistro
	                        ,p.Nome AS Produto
	                        ,sini.Sinistro AS Codigo
                            ,sit.[Descricao] AS Status
                        FROM CCSW1001V.quiver.dbo.Tabela_Documentos AS d with (nolock) 
                        JOIN CCSW1001V.quiver.dbo.Tabela_Seguradoras s with (nolock) on s.Seguradora = d.Seguradora
                        JOIN CCSW1001V.quiver.dbo.tabela_clientes AS c with (nolock) on c.cliente = d.cliente
                        INNER JOIN workflow.dbo.Tb_Seguradoras_Instrucao_Quiver AS ins with (nolock) on s.Seguradora = ins.CodSeguradora
                        LEFT JOIN CCSW1001V.quiver.dbo.Tabela_Sinistros AS sini with (nolock) on sini.Documento = d.Documento 
                        LEFT JOIN CCSW1001V.quiver.dbo.Tabela_TiposSitSinis AS sit with (nolock) on sit.Situacao_sin = sini.Situacao_sin
                        LEFT JOIN CCSW1001V.quiver.dbo.Tabela_Produtos AS p with (nolock) on p.Produto = d.Produto
                        LEFT JOIN CCSW1001V.quiver.dbo.Tabela_SegSucFones AS cf with (nolock) on cf.Seguradora = d.Seguradora
                        LEFT JOIN CCSW1001V.quiver.dbo.Tabela_SegSucContato AS ce with (nolock) on ce.Seguradora = d.Seguradora
                        WHERE d.Alteracao=0 AND ( {0} AND {1} )", param_apolice, param_cpf_cnpj);
                #endregion

                query = String.Format(@"EXEC dbo.ListaDeSinitrosParaTallos '{0}' , '{1}' ", request.apolice, request.cpf_cnpj);

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

                                var codSeguradora = dt.Rows[i][6].ToString();

                                list.Add(new ApoliceSinistroDTO()
                                {
                                    Apolice = dt.Rows[i][0].ToString(),
                                    Documento = dt.Rows[i][1].ToString(),
                                    Alteracao = dt.Rows[i][2].ToString(),
                                    Proposta_cia = dt.Rows[i][3].ToString(),
                                    Cliente = dt.Rows[i][4].ToString(),
                                    Cgc_cpf = dt.Rows[i][5].ToString(),
                                    CodSeguradora = dt.Rows[i][6].ToString(),
                                    Seguradora = dt.Rows[i][7].ToString(),
                                    Home_page = dt.Rows[i][8].ToString(),
                                    Telefone = dt.Rows[i][9].ToString(),
                                    DDD = dt.Rows[i][10].ToString(),
                                    E_mail = dt.Rows[i][11].ToString(),

                                    Instrucao2 = dt.Rows[i][12].ToString(),
                                    UrlContato = dt.Rows[i][13].ToString(),
                                    EmailContato = dt.Rows[i][14].ToString(),
                                    FoneContato = dt.Rows[i][15].ToString(),

                                    Segurado = dt.Rows[i][16].ToString(),
                                    NumeroAviso = dt.Rows[i][17].ToString(),
                                    NumeroSinistroCia = dt.Rows[i][18].ToString(),
                                    NumeroApolice = dt.Rows[i][19].ToString(),
                                    NumeroPropostaQuiver = dt.Rows[i][20].ToString(),
                                    DataDoSinistro = dt.Rows[i][21].ToString(),
                                    Produto = dt.Rows[i][22].ToString(),
                                    Codigo = dt.Rows[i][23].ToString(),
                                    Status = dt.Rows[i][24].ToString()
                                });
                                break;
                            }
                        }
                    }
                }

                string body = String.Empty;


                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        sSeguradora = item.Seguradora;
                        sHome_page = item.Home_page;
                        sTelefone = item.Telefone;
                        sDDD = item.DDD;
                        sE_mail = item.E_mail;
                        sSegurado = item.Segurado;
                        sNumeroAviso = item.NumeroAviso;
                        sNumeroSinistroCia = item.NumeroSinistroCia;
                        sNumeroApolice = item.NumeroApolice;
                        sNumeroPropostaQuiver = item.NumeroPropostaQuiver;
                        sDataDoSinistro = item.DataDoSinistro;
                        sProduto = item.Produto;
                        sCodigo = item.Codigo;
                        sStatus = item.Status;
                        instrucao2 = item.Instrucao2;
                        urlContato = item.UrlContato;
                        foneContato = item.FoneContato;
                        emailContato = item.EmailContato;
                    }
                    if (!String.IsNullOrEmpty(sNumeroSinistroCia))
                    {
                        msg = "Sucesso.";
                        erro = "";
                    }
                    else
                    {
                        msg = "Não foi encontrado nenhum registro de sinistro. Entre em contato com sua seguradora.";
                        erro = "409";
                        msg = String.Format(instrucao2, urlContato, foneContato, emailContato);
                    }
                }
                else
                {
                    msg = "Não foi encontrado nenhum registro com esse dados.";
                    erro = "409";
                }

                if (!String.IsNullOrEmpty(sDDD) && !String.IsNullOrEmpty(sTelefone))
                {
                    sTelefone = String.Format(@"({0}) {1}", sDDD, sTelefone);
                }

                if (erro == "409")
                {
                    throw new ApplicationException(msg);
                }

                result.Payload = new
                {
                    sSeguradora,
                    sHome_page,
                    sTelefone,
                    sE_mail,
                    sSegurado,
                    sNumeroAviso,
                    sNumeroSinistroCia,
                    sNumeroApolice,
                    sNumeroPropostaQuiver,
                    sDataDoSinistro,
                    sProduto,
                    sCodigo,
                    sStatus,
                    erro,
                    msg
                };

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }
    }
}
