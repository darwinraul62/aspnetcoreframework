using System;
using System.Linq;
using Ecubytes.AspNetCore.Mvc.DataTables;
using Ecubytes.Data.Common;

namespace Ecubytes.AspNetCore.Mvc.DataTables
{
    public static class DataTableQueryRequestExtensions
    {
        public static QueryRequest ToQueryRequest(this IDataTablesRequest dataTableRequest)
        {
            QueryRequest model = QueryRequest.Builder();
            if (dataTableRequest.Length > 0)
                model.Page = (dataTableRequest.Start / dataTableRequest.Length) + 1;

            model.PageSize = dataTableRequest.Length;
            model.SearchValue = dataTableRequest.Search?.Value;

            foreach (var col in dataTableRequest.Columns)
            {
                if (col.Search != null && !string.IsNullOrWhiteSpace(col.Search.Value))
                {
                    model.AddCondition(col.Field, col.Search.Value, RelationalOperators.Contain);
                }

                if (col.Sort != null)
                {
                    SortOrientation sortOrientation = SortOrientation.Ascendent;
                    if (col.Sort.Direction == SortDirection.Descending)
                        sortOrientation = SortOrientation.Descendent;

                    model.AddFieldSort(col.Field, sortOrientation, col.Sort.Order);
                }
            }

            return model;
        }
    }
}
