using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Host.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SFood.MerchantEndpoint.Host.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

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
            else if (exceptionType == typeof(BadRequestException) || exceptionType == typeof(ValidationException)) 
            {
                message = context.Exception.Message;
                status = HttpStatusCode.BadRequest;
            }
            else
            {
                message = $"Exception Message: {context.Exception.Message}";
            }
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = (int)status;
            context.Result = new JsonResult(new ApiResponse
            {
                StatusCode = BusinessStatusCode.Failed,
                Message = message
            });
        }
    }
}
