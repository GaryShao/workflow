using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFood.BusinessInfo.Common.Exceptions;
using SFood.BusinessInfo.Host.Models;
using System;
using System.Net;

namespace SFood.BusinessInfo.Host.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = string.Empty;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(BadRequestException))
            {
                message = context.Exception.ToString();
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                message = context.Exception.Message;
                status = HttpStatusCode.NotFound;
            }
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = (int)status;
            context.Result = new JsonResult(new ApiResponse {
                StatusCode = Common.Enums.BusinessStatusCode.BadRequest
            });
        }
    }
}
