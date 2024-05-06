using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Intercepts;

public class SqlLoggerIntercept(ILogger<SqlLoggerIntercept> logger) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        try
        {
            string methodName = invocation.Method.Name;
            if (methodName.StartsWith("Execute"))
            {
                var target = (IDbCommand)invocation.InvocationTarget;
                StringBuilder builder = new StringBuilder($"sql: {target.CommandText}");
                if (target.Parameters?.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendLine("parameteres:");
                    foreach (IDataParameter parameter in target.Parameters)
                    {
                        var value = parameter.Value is ICollection items ? $"{{ {string.Join(", ", items.Cast<object>().ToArray())} }}" : parameter.Value?.ToString() ;
                        builder.AppendLine($"\t@{parameter.ParameterName}={value}");
                    }

                }
                logger.LogDebug(builder.ToString());
            }
        }
        catch (Exception e)
        {
            logger.LogWarning(e.Message);
            logger.LogWarning(e.StackTrace);
        }
        invocation.Proceed();
    }
}
