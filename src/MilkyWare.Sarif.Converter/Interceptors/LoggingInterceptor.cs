﻿using MilkyWare.Sarif.Converter.Commands;
using Serilog.Core;
using Serilog.Events;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Interceptors
{
    public class LoggingInterceptor : ICommandInterceptor
    {
        public static LoggingLevelSwitch LogLevel { get; private set; } = new(LogEventLevel.Warning);

        public void Intercept(CommandContext context, CommandSettings settings)
        {
            if (settings is LoggingSettings loggingSettings)
            {
                if (loggingSettings.Debug)
                {
                    LogLevel.MinimumLevel = LogEventLevel.Verbose;
                }
            }
        }
    }
}
