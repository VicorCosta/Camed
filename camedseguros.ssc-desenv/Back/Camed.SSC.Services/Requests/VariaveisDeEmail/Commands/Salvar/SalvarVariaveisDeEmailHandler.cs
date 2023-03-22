using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Camed.SSC.Application.Requests
{
    public class SalvarVariaveisDeEmailHandler : HandlerBase, IRequestHandler<SalvarVariaveisDeEmailCommand, IResult>
    {

        private readonly IUnitOfWork uow;

        public SalvarVariaveisDeEmailHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarVariaveisDeEmailCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }


            if (request.Id == 0)
            {
                Domain.Entities.VariaveisDeEmail acao = new Domain.Entities.VariaveisDeEmail();
                try
                {
                    if (uow.GetRepository<VariaveisDeEmail>().QueryAsync(n => n.Id != request.Id && n.Nome == request.Nome).Result.Any())
                    {
                        throw new ApplicationException("Nome já cadastrado");
                    }
                }
                catch
                {
                    throw new ApplicationException("Nome já cadastrado");
                }

                acao.Nome = request.Nome;
                acao.Parametro_Id = request.ParametroId;

                await uow.GetRepository<Domain.Entities.VariaveisDeEmail>().AddAsync(acao);

            }
            else
            {
                var acao = await uow.GetRepository<Domain.Entities.VariaveisDeEmail>().GetByIdAsync(request.Id) ?? throw new ApplicationException("Nome não encontrado");

                if (uow.GetRepository<VariaveisDeEmail>().QueryAsync(n => n.Id != request.Id && n.Nome == request.Nome).Result.Any())
                {

                }

                acao.Nome = request.Nome;
                
                uow.GetRepository<Domain.Entities.VariaveisDeEmail>().Update(acao);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }
    }
}
