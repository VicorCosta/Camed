using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core;
using Camed.SSC.Core.Interfaces;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camed.SSC.WebAPI.Controllers
{
    /// <summary>
    /// Controller Base
    /// </summary>
    /// <typeparam name="TEntity">Mensagem utilizada para realizar o filtro no método GET</typeparam>
    /// <typeparam name="TCommandSave">Mensagem utilizada para realizar as ações de Inserir e Editar uma entidade</typeparam>
    /// <typeparam name="TCommandDelete">Mensagem utilizada para realizar a ação de excluir uma entidade</typeparam>
    public abstract class RestfullBaseController<TEntity, TCommandSave, TCommandDelete> : ControllerBase 
                    where TEntity : class, IEntity
                    where TCommandSave : class, IRequest<IResult>
                    where TCommandDelete : class, IRequest<IResult>
    {
        IMediator mediator;
        IUnitOfWork uow;

        /// <summary>
        /// Construtor da classe base
        /// </summary>
        /// <param name="mediator"></param>
        public RestfullBaseController(IMediator mediator, IUnitOfWork uow)
        {
            this.mediator = mediator;
            this.uow = uow;
        }

        /// <summary>
        /// Endpoint no formato OData. Esse endpoint será utilizado para retornar a listagem ou um simples objeto.
        /// </summary>
        /// <returns>Listagem de valores conforme funções do OData que serão aplicados por cada cliente</returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<TEntity>> Get()
        {
            return await uow.GetRepository<TEntity>().QueryAsync();
        }

        /// <summary>
        /// Endpoint utilizado para salvar as informações
        /// </summary>
        /// <param name="command">Objeto com os dados a serem armazenados</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IResult> Post([FromBody]TCommandSave command)
        {
            return await mediator.Send(command);
        }

        /// <summary>
        /// Endpoint utilizado para alterar as informações de uma entidade
        /// </summary>
        /// <param name="command">Objeto com os dados a serem alterados</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResult> Put([FromBody] TCommandSave command) {
            return await mediator.Send(command);
        }

        /// <summary>
        /// Endpoint utilizado para excluir uma entidade
        /// </summary>
        /// <param name="command">Objeto que possui a chave da entidade a ser excluída</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResult> Delete([FromQuery]int id)
        {
            var command = (TCommandDelete)Activator.CreateInstance(typeof(TCommandDelete), new object[] { });
            command.GetType().GetProperty("Id").SetValue(command, id);
   

            return await mediator.Send(command);
        }
    }
}