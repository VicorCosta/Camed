using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Requests.Solicitacao.Command.AdicionarAcompanhamento;
using Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirAtendente;
using Camed.SSC.Application.Requests.Solicitacao.Command.AtribuirCheckin;
using Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaApolice;
using Camed.SSC.Application.Requests.Solicitacao.Command.ConsultaSinistro;
using Camed.SSC.Application.Requests.Solicitacao.Command.CriarCopiaSolicitacao;
using Camed.SSC.Application.Requests.Solicitacao.Command.Excluir;
using Camed.SSC.Application.Requests.Solicitacao.Command.Flow;
using Camed.SSC.Application.Requests.Solicitacao.Command.InserirAcompanhamento;
using Camed.SSC.Application.Requests.Solicitacao.Command.Salvar;
using Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivo;
using Camed.SSC.Application.Requests.Solicitacao.Command.SalvarArquivoBNB;
using Camed.SSC.Application.Requests.Solicitacao.Command.SolicitacaoBNB;
using Camed.SSC.Application.ViewModel;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Camed.SSC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitacaoController : RestfullBaseController<Solicitacao, SalvarSolicitacaoCommand, ExcluirSolicitacaoCommand>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork uow;
        private readonly SSCContext context;
        public SolicitacaoController(IMediator mediator, IUnitOfWork uow, SSCContext context) : base(mediator, uow)
        {
            this.mediator = mediator;
            this.uow = uow;
            this.context = context;
        }

        [HttpGet("TESTE")]
        public async Task<IActionResult> TESTE(int solicitacaoId)
        {
            var solicitacoes = context.Solicitacoes.AsNoTracking()
                                                .Include(w => w.Solicitante)
                                                .Include(w => w.Agencia)
                                                .Include(w => w.Produtor)
                                                .Include(w => w.TipoDeProduto)
                                                .Include(w => w.CanalDeDistribuicao)
                                                .Include(w => w.TipoDeSeguro)
                                                .Include(w => w.Segurado)
                                                .Include(w => w.Segurado.VinculoBNB)
                                                .Include(w => w.Segmento)
                                                .Include(w => w.Indicacoes)
                                                .Include(w => w.AgendamentosDeLigacao)
                                                .Where(w => w.AgendamentosDeLigacao.Count > 0)
                                                .OrderByDescending(w => w.Numero)
                                                .Select(w => new
                                                {
                                                    w.Id,
                                                    w.Numero,
                                                    /*w.DataDeIngresso,
                                                    w.DadosAdicionais,
                                                    w.Origem,
                                                    w.DataFimVigencia,
                                                    w.OrcamentoPrevio,
                                                    w.Mercado,
                                                    w.OperacaoDeFinanciamento,
                                                    Solicitante = new 
                                                    {
                                                        w.Solicitante.Id,
                                                        w.Solicitante.Nome,
                                                        w.Solicitante.Email,
                                                        w.Solicitante.TelefonePrincipal,
                                                        w.Solicitante.TelefoneCelular,
                                                        w.Solicitante.TelefoneAdicional,
                                                    },
                                                    Agencia = new 
                                                    {
                                                        w.Agencia.Id,
                                                        w.Agencia.Nome,
                                                    },
                                                    Produtor = new
                                                    {
                                                        w.Produtor.Id,
                                                        w.Produtor.Nome,
                                                    },
                                                    TipoDeProduto = new 
                                                    {
                                                        w.TipoDeProduto.Id,
                                                        w.TipoDeProduto.Nome
                                                    },
                                                    CanalDeDistribuicao = new
                                                    {
                                                        w.CanalDeDistribuicao.Id,
                                                        w.CanalDeDistribuicao.Nome,
                                                    },
                                                    TipoDeSeguro = new
                                                    {
                                                        w.TipoDeSeguro.Id,
                                                        w.TipoDeSeguro.Nome,
                                                    },
                                                    Segurado = new
                                                    {
                                                        w.Segurado.Id,
                                                        w.Segurado.CpfCnpj,
                                                        w.Segurado.Nome,
                                                        w.Segurado.Email,
                                                        VinculoBNB = new
                                                        {
                                                            w.Segurado.VinculoBNB.Id,
                                                            w.Segurado.VinculoBNB.Nome,
                                                        },
                                                        w.Segurado.EmailSecundario,
                                                        w.Segurado.TelefonePrincipal,
                                                        w.Segurado.TelefoneCelular,
                                                        w.Segurado.TelefoneAdicional,
                                                        w.Segurado.Contato,
                                                    },
                                                    Segmento = new
                                                    {
                                                        w.Segmento.Id,
                                                        w.Segmento.Nome,
                                                    },
                                                    AgenciaConta = new
                                                    {
                                                        w.AgenciaConta.Id,
                                                        w.AgenciaConta.Nome
                                                    },*/
                                                    w.AgendamentosDeLigacao,
                                                });
            return Ok(new { count = solicitacoes.Count(), solicitacoes = (await solicitacoes.ToListAsync()).Take(5).Skip(0) });
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IResult> Post(PostSolicitacaoCommand command)
        {
            return await mediator.Send(command);
        }
        
        [HttpPost]
        [Route("Flow")]
        public async Task<IResult> Flow(FlowSolicitacaoCommand command)
        {
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("FormatarArquivo")]
        public async Task<IResult> SalvarArquivo([FromQuery]int solicitacaoId, List<IFormFile> files)
         {
            /*var command = new FormatarArquivoSolicitacaoCommand();
            command.FormFiles = anexos;
            return await mediator.Send(command);*/

            var novosArquivos = new FormatarArquivoSolicitacaoCommand();

            novosArquivos.Solicitacao_Id = solicitacaoId;
            novosArquivos.FormFiles = new List<AnexoDeSolicitacaoViewModel>();
            foreach (var file in files)
            {
                var command = new AnexoDeSolicitacaoViewModel();

                command.Extensao = new FileInfo(file.FileName).Extension;
                command.Nome = new FileInfo(file.FileName).Name.Replace(command.Extensao, "");
                BinaryReader b = new BinaryReader(file.OpenReadStream());
                byte[] binData = b.ReadBytes(file.Length.GetHashCode());
                command.Arquivo = binData;
                novosArquivos.FormFiles.Add(command);
            }

            return await mediator.Send(novosArquivos);
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> BaixarArquivo(int idArquivo)
        {
            var caminhoAnexos = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Parametro == "CAMINHORAIZANEXOS").Result.Valor;
            string link = "";
            var anexo = uow.GetRepository<AnexoDeSolicitacao>().QueryFirstOrDefaultAsync(w => w.Id == idArquivo).Result;
            link = caminhoAnexos + anexo.Caminho;
            var fileBytes = System.IO.File.ReadAllBytes(link);

            var idxPonto = anexo.Caminho.LastIndexOf(".");
            var extensao = anexo.Caminho.Substring(idxPonto);

            /*var extensao = new FileInfo(anexo.Nome).Extension;*/

            return new FileContentResult(fileBytes, "application/" + extensao)
            {
                FileDownloadName = anexo.Nome + extensao
            };
        }
       
        [HttpPost]
        [Route("InserirSolicitacao")]
        public async Task<IResult> SalvarSolicitacaoBanco(SolicitacaoBNBCommand solicitacaoBNB)
        {
            return await mediator.Send(solicitacaoBNB);
        }

        [HttpPost]
        [Route("InserirAcompanhamento")]
        public async Task<IResult> SalvarAcompanhamentoBanco(InserirAcompanhamentoCommand command)
        {
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("SalvarArquivosBNB")]
        public async Task<IResult> SalvarArquivoBNB(List<IFormFile> files)
        {
            var novosArquivos = new SalvarArquivoBNBCommand();
            novosArquivos.anexos = new List<AnexoBNBViewModel>();
            foreach (var file in files)
            {
                var command = new AnexoBNBViewModel();

                command.Extensao = new FileInfo(file.FileName).Extension;
                command.NomeDoArquivo = new FileInfo(file.FileName).Name.Replace(command.Extensao, "");
                BinaryReader b = new BinaryReader(file.OpenReadStream());
                byte[] binData = b.ReadBytes(file.Length.GetHashCode());
                command.ConteudoBase64 = binData.ToString();
                novosArquivos.anexos.Add(command);
            }

            return await mediator.Send(novosArquivos);
        }

        [HttpPost]
        [Route("criarCopia")]
        public async Task<IResult> CriarCopia(CriarCopiaSolicitacaoCommand solicitacao)
        {
            return await mediator.Send(solicitacao);        
        }

        [HttpPost]
        [Route("adicionarAcompanhamento")]
        public async Task<IResult> AdicionarAcompanhamento(AcompanhamentoViewModel acompanhamento, List<IFormFile> files)
         {
            var command = new AdicionarAcompanhamentoCommand();
            acompanhamento.Anexos = new List<AnexoDeAcompanhamento>();

            foreach(var file in files)
            {
                acompanhamento.Anexos.Add(new AnexoDeAcompanhamento() {
                    Nome = new FileInfo(file.FileName).FullName
                });
            }
            command.acompanhamento = acompanhamento;
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("atribuirAtendente")]
        public async Task<IResult> AtribuirAtendente(AtribuirAtendenteCommand command)
        {
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("atribuirCheckin")]
        public async Task<IResult> AtribuirCheckin(AtribuirCheckinCommand command)
        {
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("consultaApoliceBoleto")]
        public async Task<IResult> ConsultaApoliceBoleto(ConsultaApoliceCommand command)
        {
            command.Link = Request.GetDisplayUrl(); //A testar
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("consultaSinistro")]
        public async Task<IResult> ConsultaSinistro(ConsultaSinistroCommand command)
        {
            return await mediator.Send(command);
        }
    }
}
