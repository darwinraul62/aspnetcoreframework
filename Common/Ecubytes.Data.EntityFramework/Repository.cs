using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ecubytes.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ecubytes.Data.EntityFramework
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected DbSet<T> dbSet;

        public IUnitOfWork UnitOfWork => Context;

        public Repository(DbContext context)
        {
            this.Context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<Common.QueryResult<T>> GetAsync(QueryRequest request, 
            Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            var queryable = GetQueryable(filter, orderBy, includeProperties);

            QueryResult<T> response = new QueryResult<T>();

            var query = queryable.ApplyQueryRequest(request);
            var queryCount = queryable.ApplyQueryRequestCount(request);

            response.Page = request.Page;
            response.Data = await query.ToListAsync();

            response.TotalCount = await queryCount.CountAsync();

            return response;
        }

        private async Task<Common.QueryResult<T>> GetAsync(IQueryable<T> queryable, QueryRequest request)
        {
            QueryResult<T> response = new QueryResult<T>();

            var query = queryable.ApplyQueryRequest(request);
            var queryCount = dbSet.AsQueryable().ApplyQueryRequestCount(request);

            response.Page = request.Page;
            response.Data = await query.ToListAsync();

            response.TotalCount = await queryCount.CountAsync();

            return response;
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            IQueryable<T> result = query;

            if (orderBy != null)
            {
                result = orderBy(query);
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<T> queryable = GetQueryable(filter, orderBy, includeProperties);

            return await queryable.ToListAsync();
        }

        // public async Task<List<T>> ToListFromQueryableAsync(IQueryable<T> source)
        // {
        //     return await source.ToListAsync();
        // }

        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefaultAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            return dbSet.AnyAsync(filter);
        }

        public T Get(int id)
        {            
            return dbSet.Find(id);
        }

        public void Add(T entity)
        {            
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }
        public void Remove(params object[] keyValues)
        {
            T entityToRemove = dbSet.Find(keyValues);
            Remove(entityToRemove);
        }        
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {            
            dbSet.RemoveRange(entities);
        }

        // public void Dispose()
        // {
        //     if(UnitOfWork!=null)
        //     {
        //         UnitOfWork.Dispose();                
        //     }
        //     this.dbSet = null;            
        // }
    }
}