using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Intercepts;

public class DbConnectionIntercept(IProxyGenerator proxyGenerator,
                             SqlLoggerIntercept sqlLoggerIntercept) : IInterceptor
{


    public void Intercept(IInvocation invocation)
    {
        string methodName = invocation.Method.Name;
        invocation.Proceed();

        if (methodName == "CreateDbCommand")
        {
            var target = (DbCommand)invocation.ReturnValue;
            var proxy = proxyGenerator.CreateClassProxyWithTarget(target, sqlLoggerIntercept);
            invocation.ReturnValue = proxy;
        }
    }
}
