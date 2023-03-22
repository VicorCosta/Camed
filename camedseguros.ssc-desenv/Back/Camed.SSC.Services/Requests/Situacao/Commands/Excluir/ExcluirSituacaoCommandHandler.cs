using AutoMapper;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class ExcluirSituacaoCommandHandler : HandlerBase, IRequestHandler<ExcluirSituacaoCommand, IResult>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork uow;

        public ExcluirSituacaoCommandHandler(IMapper mapper, IUnitOfWork uow)
        {
            this.mapper = mapper;
            this.uow = uow;
        }

        public async Task<IResult> Handle(ExcluirSituacaoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            var situacao = await uow.GetRepository<Domain.Entities.Situacao>().GetByIdAsync(request.Id);

            if(situacao != null)
            {
                var fk_situacao = await uow.GetRepository<Domain.Entities.TipoDeProduto>().QueryFirstOrDefaultAsync(u => u.Situacao_Id == situacao.Id);
                if (fk_situacao != null)
                {
                    throw new ApplicationException($"Situação contém relação com o tipo de produto: '{string.Join(", ", fk_situacao.Nome)}'");
                }
                else
                {
                    await uow.GetRepository<Domain.Entities.Situacao>().RemoveAndSaveAsync(situacao);
                }
            }
            else
            {
                result.Message = "Situação não localizada na base de dados";
                return await Task.FromResult(result);
            }

            
            return await Task.FromResult(result);
        }
    }
}
