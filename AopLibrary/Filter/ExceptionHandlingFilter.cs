using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Filter;

public class ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        //　異常ログを出力
        logger.LogError(context.Exception, context.Exception.Message);

        string errorMessage = context.Exception.Message.Replace("\r"," ").Replace("\n"," ");

        context.HttpContext.Response.Headers.Add("ErrorMessage",errorMessage);
        context.ExceptionHandled = true;
    }
}
