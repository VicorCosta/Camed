using Camed.SSC.Core.Interfaces;
using Camed.SSC.Domain.Entities;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Camed.SCC.Infrastructure.Data.Interfaces
{
    public interface IUnitOfWork
    {
        
        IQueryable<T> ExecuteQuery<T>(string command, params SqlParameter[] parameters) where T : class;
        IRepository<TEntity> GetRepository<TEntity>() where TEntity  : class, IEntity;
        Task<bool> CommitAsync();
        bool Commit();


    }
}
