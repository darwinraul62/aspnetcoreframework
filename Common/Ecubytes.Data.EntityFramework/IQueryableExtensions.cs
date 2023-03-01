using System;
using System.Linq;
using System.Threading.Tasks;
using Ecubytes.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Data.EntityFramework
{
    public static class IQueryableExtensions
    {
        public static async Task<QueryResult<TSource>> GetQueryResultAsync<TSource>(this IQueryable<TSource> queryable, 
            QueryRequest request) 
            where TSource : class
        {            
            QueryResult<TSource> response = new QueryResult<TSource>();

            var query = queryable.ApplyQueryRequest(request);
            var queryCount = queryable.ApplyQueryRequestCount(request);

            response.Page = request.Page;
            response.Data = await query.ToListAsync();

            response.TotalCount = await queryCount.CountAsync();

            return response;
        }
    }
}
