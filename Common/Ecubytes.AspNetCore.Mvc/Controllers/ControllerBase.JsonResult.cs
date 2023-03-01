using System;
using Microsoft.AspNetCore.Mvc;

namespace Ecubytes.AspNetCore.Mvc.Controllers
{
    public partial class ControllerBase
    {      
        //
        // Resumen:
        //     Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified
        //     data object to JSON.
        //
        // Parámetros:
        //   data:
        //     The object to serialize.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.JsonResult that serializes the specified
        //     data to JSON format for the response using ModelResult Object.     
        [NonAction]
        public virtual JsonResult JsonModelResult(object data, string redirectUrl)
        {
            return base.Json(GetJsonModelResult(data, redirectUrl));
        }

        //
        // Resumen:
        //     Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified
        //     data object to JSON.
        //
        // Parámetros:
        //   data:
        //     The object to serialize.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.JsonResult that serializes the specified
        //     data to JSON format for the response using ModelResult Object.     
        [NonAction]
        public virtual JsonResult JsonModelResult(object data)
        {
            return base.Json(GetJsonModelResult(data));
        }

        //
        // Resumen:
        //     Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified
        //     data object to JSON.
        //
        // Parámetros:
        //   data:
        //     The object to serialize.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.JsonResult that serializes the ModelResult Object
        //     to JSON format for the response.     
        [NonAction]
        public virtual JsonResult JsonModelResult(string redirectUrl)
        {
            return base.Json(GetJsonModelResult(redirectUrl));
        }

        //
        // Resumen:
        //     Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified
        //     data object to JSON.
        //
        // Parámetros:
        //   data:
        //     The object to serialize.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.JsonResult that serializes the ModelResult Object
        //     to JSON format for the response.     
        [NonAction]
        public virtual JsonResult JsonModelResult()
        {
            return base.Json(GetJsonModelResult());
        }
    }
}
