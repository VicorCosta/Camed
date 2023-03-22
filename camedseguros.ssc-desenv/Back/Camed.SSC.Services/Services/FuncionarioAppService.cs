using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Application.Interfaces;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Services
{
    public class FuncionarioAppService : IFuncionarioAppService
    {
        private readonly IUnitOfWork uow;

        public FuncionarioAppService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public Funcionario GetFuncionarioPorMatricula(string matricula, bool replace)
        {
            if (replace)
                matricula = matricula.Replace("F", "").Replace("f", "");
            return uow.GetRepository<Funcionario>()
                .QueryFirstOrDefaultAsync(x => x.Matricula.Equals(matricula)).Result;
        }
    }
}
