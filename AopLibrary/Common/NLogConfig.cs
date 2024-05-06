using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Common;

public static class NLogConfig
{

    private const string LOG_MESSAGE_LAYOUT = "${longdate_utc} | ${aspnet-sessinId} | ${level} | ${logger} | ${message} ${exception:format=tostring}";

    private const string LOG_FILE_NAME = "WebSample.${date:format=yyyy-MM-dd}.log";

    public static void Initialize()
    {
        LogManager.Setup().SetupExtensions(e =>
        {
            e.RegisterAssembly("NLog.Web.AspNetCore");
            e.RegisterLayoutRenderer("longdate_utc", (logEvent) => logEvent.TimeStamp.ToUniversalTime().ToString("O"));
        });

        var config = new LoggingConfiguration();
        LogLevel logLevel = LogLevelSetting();

        config.AddRule(logLevel, NLog.LogLevel.Fatal, ConfigConsoleTarget());

        // File
        config.AddRule(logLevel, NLog.LogLevel.Fatal, ConfigFileTarget());


        LogManager.Configuration = config;
    }

    private static Target ConfigConsoleTarget()
    {

        var consoleTarget = new ConsoleTarget { };
        return new AsyncTargetWrapper
        {
            WrappedTarget = consoleTarget,
            OverflowAction = AsyncTargetWrapperOverflowAction.Grow
        };
    }

    private static Target ConfigFileTarget()
    {

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var logsDir = Path.Combine(baseDir, "logs");

        var fileTarget = new FileTarget {
            Encoding = Encoding.UTF8,
            Name = "fileTarget",
            Layout = LOG_MESSAGE_LAYOUT,
            FileName = Path.Combine(logsDir, LOG_FILE_NAME)
        };
        return new AsyncTargetWrapper
        {
            WrappedTarget = fileTarget,
            OverflowAction = AsyncTargetWrapperOverflowAction.Grow
        };
    }




    private static LogLevel LogLevelSetting()
    {
        string logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "";
        return logLevel.ToLower() switch
        {
            "trace" => LogLevel.Trace,
            "debug" => LogLevel.Debug,
            "info" => LogLevel.Info,
            "warn" => LogLevel.Warn,
            "error" => LogLevel.Error,
            "fatal" => LogLevel.Fatal,
            _ => LogLevel.Debug,
        };
    }
}
