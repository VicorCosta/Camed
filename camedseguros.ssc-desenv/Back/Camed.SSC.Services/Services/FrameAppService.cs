using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Services
{
    public class FrameAppService : IFrameAppService
    {
        private readonly IUnitOfWork uow;

        public FrameAppService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public Frame ListaPorNome(string frameNome)
        {
            return uow.GetRepository<Frame>().QueryFirstOrDefaultAsync(x => x.Nome.Equals(frameNome), includes: new[] { "AcoesAcompanhamento" }).Result;
        }
    }
}
