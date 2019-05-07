using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.ClientEndpoint.Common.Extensions;
using SFood.ClientEndpoint.Host.Attributes;
using SFood.ClientEndpoint.Host.Controllers;
using System;
using System.Linq;

namespace SFood.ClientEndpoint.Host.Filters
{
    public class TransactionActionFilter : IActionFilter
    {
        private readonly IRepository _repository;
        private bool _isTransactional = true;

        public TransactionActionFilter(IRepository repository)
        {
            _repository = repository;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseController;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            //if (IsActionNeedAuthorized(controllerActionDescriptor) && (controller.RestaurantId.IsNullOrEmpty() ||
            //    controller.UserId.IsNullOrEmpty() ||
            //    controller.Role.IsNullOrEmpty()))
            //{
            //    throw new UnauthorizedAccessException("Bad Token : Information Missing");
            //}            

            if (controllerActionDescriptor != null)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: false);
                if (!actionAttributes.Any(attr => attr is TransactionalAttribute ||
                    attr is HttpPostAttribute || attr is HttpPutAttribute ||
                    attr is HttpDeleteAttribute))
                {
                    _isTransactional = false;
                } 
            }                    
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_isTransactional)
            {
                _repository.Save(); 
            }            
        }     

        private bool IsActionNeedAuthorized(ControllerActionDescriptor controllerActionDescriptor)
        {
            var isActionMarkedAsAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes(false).Any(attr => attr is AllowAnonymousAttribute);
            return !isActionMarkedAsAnonymous;
        }
    }
}
