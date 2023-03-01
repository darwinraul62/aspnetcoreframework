using System;
using Ecubytes.AspNetCore.Mvc.Models;
using Ecubytes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Ecubytes.AspNetCore.Mvc.Controllers
{
    public partial class ControllerBase
    {
        [NonAction]
        public OkObjectResult OkModelResult([ActionResultObjectValue] object value)
        {
            return base.Ok(GetModelResult(value));
        }
        [NonAction]
        public OkObjectResult OkModelResult(string message)
        {
            this.AddSuccessMessage(message);
            return base.Ok(GetModelResult());
        }
        [NonAction]
        public OkObjectResult OkModelResult()
        {
            return base.Ok(GetModelResult());
        }
        [NonAction]
        public NotFoundObjectResult NotFoundModelResult([ActionResultObjectValue] object value)
        {
            return base.NotFound(GetModelResult(value));
        }
        [NonAction]
        public NotFoundObjectResult NotFoundModelResult()
        {
            if (this.Messages.Count == 0)
                this.AddErrorMessage("The requested resource was not found");

            return base.NotFound(GetModelResult());
        }
        [NonAction]
        public NotFoundObjectResult NotFoundModelResult(string message)
        {
            this.AddErrorMessage(message);
            return NotFoundModelResult();
        }
        [NonAction]
        public ObjectResult InternalServerErrorModelResult([ActionResultObjectValue] object value)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, GetModelResult(value));
        }

        [NonAction]
        public ObjectResult InternalServerErrorModelResult(string message)
        {
            this.AddErrorMessage(message);
            return StatusCode(StatusCodes.Status500InternalServerError, GetModelResult());
        }

        [NonAction]
        public ObjectResult InternalServerErrorModelResult(Exception ex, string message)
        {
            this.AddErrorMessage(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, GetModelResult());
        }
        [NonAction]
        public ObjectResult InternalServerErrorModelResult(Exception ex)
        {
            this.AddErrorMessage(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, GetModelResult());
        }
        [NonAction]
        public ObjectResult InternalServerErrorModelResult()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, GetModelResult());
        }
        [NonAction]
        public ObjectResult InternalServerError(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
        

        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.
        //
        // Parámetros:
        //   error:
        //     An error object to be returned to the client.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response using ModelResult Object.      
        [NonAction]
        public BadRequestObjectResult BadRequestModelResult([ActionResultObjectValue] object error)
        {
            return base.BadRequest(GetModelResult(error));
        }
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.    
        //
        // Parámetros:
        //   message:
        //     A error message to include in the ModelResult as part of the response          
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response using ModelResult Object.     
        [NonAction]
        public BadRequestObjectResult BadRequestModelResult()
        {
            return base.BadRequest(GetModelResult());
        }
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.BadRequestObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        //     response.              
        //
        // Parámetros:
        //   message:
        //     A error message to include in the ModelResult as part of the response       
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.BadRequestObjectResult for the response using ModelResult Object. 
        [NonAction]
        public BadRequestObjectResult BadRequestModelResult(string message)
        {
            this.AddErrorMessage(message);
            return BadRequestModelResult();            
        }    
        
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.
        //
        // Parámetros:
        //   error:
        //     An error object to be returned to the client.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response using ModelResult Object.      
        [NonAction]
        public ConflictObjectResult ConflictModelResult([ActionResultObjectValue] object error)
        {
            return base.Conflict(GetModelResult(error));
        }
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.                       
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response using ModelResult Object.     
        [NonAction]
        public ConflictObjectResult ConflictModelResult()
        {
            return base.Conflict(GetModelResult());
        }
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.ConflictObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict
        //     response.              
        //
        // Parámetros:
        //   message:
        //     A error message to include in the ModelResult as part of the response       
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.ConflictObjectResult for the response using ModelResult Object. 
        [NonAction]
        public ConflictObjectResult ConflictModelResult(string message)
        {
            this.AddErrorMessage(message);
            return ConflictModelResult();
        }


         //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityResult that produces a
        //     Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity response.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityResult for the Model Result response
        [NonAction]
        public UnprocessableEntityObjectResult UnprocessableEntityModelResult([ActionResultObjectValue] object error)
        {
            return base.UnprocessableEntity(GetModelResult(error));
        }
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity
        //     response.             
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityResult for the response using ModelResult Object.     
        [NonAction]
        public UnprocessableEntityObjectResult UnprocessableEntityModelResult()
        {
            return base.UnprocessableEntity(GetModelResult());
        }
        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.UnprocessableEntityResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity
        //     response.             
        //
        // Parámetros:
        //   message:
        //     A error message to include in the ModelResult as part of the response       
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.UnprocessableEntityResult for the Model Result response
        [NonAction]
        public UnprocessableEntityObjectResult UnprocessableEntityModelResult(string message)
        {
            this.AddErrorMessage(message);
            return UnprocessableEntityModelResult();
        }


        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
        //     response.
        //
        // Parámetros:
        //   error:
        //     An error object to be returned to the client.
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response using ModelResult Object.      
        [NonAction]
        public UnauthorizedObjectResult UnauthorizedModelResult([ActionResultObjectValue] object error)
        {
            return base.Unauthorized(GetModelResult(error));
        }

        //
        // Resumen:
        //     Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
        //     response.    
        //
        // Parámetros:
        //   message:
        //     A error message to include in the ModelResult as part of the response          
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response using ModelResult Object.     
        [NonAction]
        public UnauthorizedObjectResult UnauthorizedModelResult()
        {
            return base.Unauthorized(GetModelResult());
        }
        //
        // Resumen:
        // Creates an Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult that produces a Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
        //     response.              
        //
        // Devuelve:
        //     The created Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult for the response using ModelResult Object. 
        [NonAction]
        public UnauthorizedObjectResult UnauthorizedModelResult(string message)
        {
            this.AddErrorMessage(message);
            return UnauthorizedModelResult();
        }

        private ModelResult<T> GetModelResult<T>(T data)
        {
            ModelResult<T> modelResult = new ModelResult<T>();
            modelResult.Messages.AddRange(this.Messages);
            modelResult.Data = data;
            return modelResult;
        }

        private ModelResult GetModelResult()
        {
            ModelResult modelResult = new ModelResult();
            modelResult.Messages.AddRange(this.Messages);

            return modelResult;
        }

        private JsonModelResult<T> GetJsonModelResult<T>(T data, string redirectUrl = null)
        {
            JsonModelResult<T> modelResult = new JsonModelResult<T>();
            modelResult.Messages.AddRange(this.Messages);
            modelResult.Data = data;
            modelResult.RedirectUrl = redirectUrl;
            return modelResult;
        }

        private JsonModelResult GetJsonModelResult(string redirectUrl = null)
        {
            JsonModelResult modelResult = new JsonModelResult();
            modelResult.Messages.AddRange(this.Messages);
            modelResult.RedirectUrl = redirectUrl;

            return modelResult;
        }
    }
}
