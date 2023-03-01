using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecubytes.Data.Common;

namespace Ecubytes.Data
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IQueryable<T> GetQueryable(
            Expression<Func<T,bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null            
        );
        //Task<List<T>> ToListFromQueryableAsync(IQueryable<T> source);

        Task<Common.QueryResult<T>> GetAsync(QueryRequest request, 
            Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);
        
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T,bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null            
        );
        Task<bool> ExistsAsync(
            Expression<Func<T,bool>> filter = null     
        );
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T,bool>> filter = null,
            string includeProperties = null
        );

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(params object[] keyValues);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        IUnitOfWork UnitOfWork { get; }
    }
}