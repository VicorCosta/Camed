using Camed.SSC.Core.Enums;
using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camed.SCC.Infrastructure.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> QueryFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null, string orderBy = null, SortDirection? sortDirection = null, params string[] includes);


        Task AddAsync(TEntity entity);
        Task AddAndSaveAsync(TEntity entity);

        void Update(TEntity entity);
        Task UpdateAndSaveAsync(TEntity entity);

        void Remove(TEntity entity);
        Task RemoveAndSaveAsync(TEntity entity);
    }
}
