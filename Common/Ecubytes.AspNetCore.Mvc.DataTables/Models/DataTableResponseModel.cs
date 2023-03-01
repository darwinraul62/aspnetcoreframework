using System;
using Ecubytes.AspNetCore.Mvc.DataTables;

namespace Ecubytes.Extensions.AspNetCore.Mvc.DataTable.Models
{
    public class DataTableResponseModel
    {
        public IDataTablesRequest Request { get; set; }
        public object Data { get; set; }
        public int TotalRecordsFiltered { get; set; }
        public int TotalRecords { get; set; }
        public int Page { get; set; }
    }
}
