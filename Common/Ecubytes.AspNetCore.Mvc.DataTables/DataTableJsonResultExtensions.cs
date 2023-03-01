using System;
using Ecubytes.AspNetCore.Mvc.Controllers;
using Ecubytes.Extensions.AspNetCore.Mvc.DataTable.Models;

namespace Ecubytes.AspNetCore.Mvc.DataTables
{
    public static class DataTableJsonResultExtensions
    {        
        public static DataTablesJsonResult DataTableJsonResult(this ControllerBase controller,
            DataTableResponseModel model)
        {            
            return DataTableJsonResult(model);
        }

        public static DataTablesJsonResult DataTableJsonResult(DataTableResponseModel model)
        {
            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(model.Request, 
                model.TotalRecords, model.TotalRecordsFiltered, model.Data);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, true);
        }
    }
}
