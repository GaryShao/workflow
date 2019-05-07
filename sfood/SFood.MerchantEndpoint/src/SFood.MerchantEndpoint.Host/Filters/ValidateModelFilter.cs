using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SFood.MerchantEndpoint.Host.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        private readonly ILogger<ValidateModelFilter> _logger;

        public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //do nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var errors = context.ModelState.Select(x => x.Value.Errors)
                .Where(y => y.Count > 0)
                .ToList();

            var detail = string.Join(Environment.NewLine, errors.SelectMany(x => x.Select(y => y.ErrorMessage)));
            throw new ValidationException(detail);
        }
    }
}
