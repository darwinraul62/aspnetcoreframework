using System;
using System.Collections.Generic;

namespace Ecubytes.Data.Common
{
    public abstract class QueryResultBase
    {
        public int? Page { get; set; }
        public int TotalCount { get; set; }
    }

    public class QueryResult<T> : QueryResultBase where T : class
    {
        public IEnumerable<T> Data { get; set; }
    }

    public class QueryResult : QueryResult<object>
    {
        public static QueryResult<T> Convert<T>(QueryResultBase baseModel, IEnumerable<T> data) where T : class
        {
            var response = new QueryResult<T>();
            response.Page = baseModel.Page;
            response.TotalCount = baseModel.TotalCount;
            response.Data = data;

            return response;
        }
    }
}
