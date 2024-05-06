using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Filter;

public class LoggerActionFilter(ILogger<LoggerActionFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        logger.LogDebug($"LogerActionFilter Extending {context.ActionDescriptor.DisplayName}");
        await next();
        logger.LogDebug($"LogerActionFilter Extended {context.ActionDescriptor.DisplayName}");
    }
}
