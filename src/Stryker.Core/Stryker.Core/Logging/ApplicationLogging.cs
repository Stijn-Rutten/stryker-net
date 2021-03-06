﻿using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;
using System.IO;

namespace Stryker.Core.Logging
{
    public static class ApplicationLogging
    {
        private static ILoggerFactory _factory = null;

        public static void ConfigureLogger(LogOptions options, IEnumerable<LogMessage> initialLogMessages = null)
        {
            LoggerFactory.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Is(options.LogLevel)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger());

            if (options.LogToFile)
            {
                // log on the lowest level to the log file
                var logFilesPath = Path.Combine(options.OutputPath, "logs");
                LoggerFactory.AddFile(logFilesPath + "/log-{Date}.txt", LogLevel.Trace);
            }

            if (initialLogMessages != null)
            {
                var logger = LoggerFactory.CreateLogger("InitialLogging");

                foreach (var logMessage in initialLogMessages)
                {
                    logger.Log(logMessage.LogLevel, logMessage.EventId, logMessage.State, logMessage.Exception, logMessage.Formatter);
                }
            }
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new LoggerFactory();
                }
                return _factory;
            }
            set { _factory = value; }
        }
    }
}
