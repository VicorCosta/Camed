using Camed.SCC.Infrastructure.Data.Context;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Core.Data;
using Camed.SSC.Core.Enums;
using Camed.SSC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Camed.SCC.Infrastructure.Data
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly SSCContext context;

        public Repository(SSCContext context)
        {
            this.context = context;
        }

        #region Commands

        public async Task AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);
        }

        public async Task AddAndSaveAsync(TEntity entity)
        {
            await AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            context.Update(entity);
        }

        public async Task UpdateAndSaveAsync(TEntity entity)
        {
            Update(entity);
            await context.SaveChangesAsync();
        }

        public void Remove(TEntity entity)
        {
            context.Remove(entity);
        }

        public async Task RemoveAndSaveAsync(TEntity entity)
        {
            Remove(entity);
            await context.SaveChangesAsync();
        }


        #endregion

        #region Queries

        public Task<TEntity> GetByIdAsync(int id)
        {
            return context.Set<TEntity>().FindAsync(id);
        }

        public Task<IQueryable<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            SortDirection? sortDirection = null,
            params string[] includes)
        {
            var query = context.Set<TEntity>() as IQueryable<TEntity>;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = ApplyIncludes(query, includes);
            }

            if (orderBy != null)
            {
                query = ApplyOrderBy(query, new OrderByInfo { Direction = (sortDirection ?? SortDirection.Ascending), Initial = true, PropertyName = orderBy });
            }

            return Task.FromResult(query);
        }

        public async Task<TEntity> QueryFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate, 
            params string[] includes)
        {
            var query = await QueryAsync(filter: predicate, includes: includes);

            if (query.Any())
            {
                return await query.FirstAsync();
            }

            return null;
        }

        #endregion

        #region Utils

        IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params string[] includes)
        {
            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }
        IQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query, OrderByInfo orderByInfo)
        {
            string[] props = orderByInfo.PropertyName.Split('.');
            Type type = typeof(TEntity);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            string methodName = String.Empty;

            if (!orderByInfo.Initial && query is IOrderedQueryable<TEntity>)
            {
                if (orderByInfo.Direction == SortDirection.Ascending)
                    methodName = "ThenBy";
                else
                    methodName = "ThenByDescending";
            }
            else
            {
                if (orderByInfo.Direction == SortDirection.Ascending)
                    methodName = "OrderBy";
                else
                    methodName = "OrderByDescending";
            }

            return (IOrderedQueryable<TEntity>)typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), type)
                .Invoke(null, new object[] { query, lambda });

        }

        #endregion
    }
}
