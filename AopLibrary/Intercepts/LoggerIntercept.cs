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
        logger.LogInformation($"LoggerIntercept Executing {invocation.MethodInvocationTarget}");
        invocation.Proceed();
        logger.LogInformation($"LoggerIntercept Executed {invocation.MethodInvocationTarget}");
    }
}

