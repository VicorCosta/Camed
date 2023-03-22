using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Camed.SCC.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SSCContext context;
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(SSCContext context)
        {
            this.context = context;
        }


        public async Task<bool> CommitAsync()
        {
            return await context.SaveChangesAsync() > 1;
        }

        public bool Commit()
        {
            return context.SaveChanges() > 1;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            var tp = typeof(TEntity);

            if (repositories.ContainsKey(tp) == false)
            {
                repositories[tp] = new Repository<TEntity>(context);
            }

            return (IRepository<TEntity>)repositories[tp];
        }

        public IQueryable<T> ExecuteQuery<T>(string query, params SqlParameter[] parameters)
            where T : class
        {
            return this.context.Query<T>().FromSql(query, parameters);
        }

    }
}
