using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Intercepts;

public class LoggerIntercept(ILogger<LoggerIntercept> logger) : IInterceptor
{
 

    public void Intercept(IInvocation invocation)
    {
        logger.LogDebug($"LoggerIntercept Executing {invocation.MethodInvocationTarget}");
        invocation.Proceed();
        logger.LogDebug($"LoggerIntercept Executed {invocation.MethodInvocationTarget}");
    }
}

