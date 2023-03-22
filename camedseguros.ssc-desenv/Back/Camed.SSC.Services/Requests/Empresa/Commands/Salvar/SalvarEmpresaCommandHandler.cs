using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Extensions;
using Camed.SSC.Core.Handlers;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Camed.SSC.Application.Requests
{
    public class SalvarEmpresaCommandHandler : HandlerBase, IRequestHandler<SalvarEmpresaCommand, IResult>
    {
        private readonly IUnitOfWork uow;

        public SalvarEmpresaCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IResult> Handle(SalvarEmpresaCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                base.result.SetValidationErros(request);
                return await Task.FromResult(result);
            }

            Domain.Entities.Empresa empresa = await uow.GetRepository<Domain.Entities.Empresa>().QueryFirstOrDefaultAsync(r => r.Id == request.Id, includes: new[] { "TiposDeSeguro" }) ?? new Domain.Entities.Empresa();
            empresa.Nome = request.Nome;

            if (request.TiposDeSeguro.Any())
            {
                var tiposDeSeguroRequest = request.TiposDeSeguro.Select(s => s.TipoSeguro_Id).ToArray();

                //Atualizacao do atributo Permitido Abrir
                foreach (var itemDoBanco in empresa.TiposDeSeguro.Where(w => tiposDeSeguroRequest.Contains(w.TipoDeSeguro_id)))
                {
                    itemDoBanco.Permitido_Abrir = (request.TiposDeSeguro.FirstOrDefault(f => f.TipoSeguro_Id == itemDoBanco.TipoDeSeguro_id).Permitido_Abrir);
                }

                //Removendo tipo De Seguro
                empresa.TiposDeSeguro.RemoveAll(w => !tiposDeSeguroRequest.Contains(w.TipoDeSeguro_id));


                //Adicionar Tipo de Seguro
                var tiposDeSeguroBanco = empresa.TiposDeSeguro.Select(s => s.TipoDeSeguro_id).ToArray();
                foreach (var item in request.TiposDeSeguro.Where(w => !tiposDeSeguroBanco.Contains(w.TipoSeguro_Id)))
                {
                    empresa.TiposDeSeguro.Add(new EmpresaTipoDeSeguro { TipoDeSeguro_id = item.TipoSeguro_Id, Permitido_Abrir = item.Permitido_Abrir });
                }
            }

            if (request.Id == 0)
            {
                await uow.GetRepository<Domain.Entities.Empresa>().AddAsync(empresa);
            }
            else
            {
                uow.GetRepository<Domain.Entities.Empresa>().Update(empresa);
            }

            await uow.CommitAsync();

            return await Task.FromResult(result);
        }

    }
}
