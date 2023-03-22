using AutoMapper;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests.Inbox.Commands.Excluir
{
    public class ExcluirInboxCommandHandler : HandlerBase, IRequestHandler<ExcluirInboxCommand, IResult>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public ExcluirInboxCommandHandler(IMapper mapper, IUnitOfWork uow)
        {
            this.mapper = mapper;
            this.uow = uow;
        }
        public async Task<IResult> Handle(ExcluirInboxCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }
            var mensagemBanco = await uow.GetRepository<Domain.Entities.Inbox>().GetByIdAsync(request.Id);

            if (mensagemBanco != null)
            {
                mensagemBanco.VisivelEntrada = false;
                mensagemBanco.VisivelSaida = false;
                uow.Commit();
            }
            else
            {
                throw new ApplicationException("Menssagem não encontrada");
            }

            return await Task.FromResult(result);
        }
    }
}
