using Microsoft.Extensions.DependencyInjection;
using MilkyWare.Sarif.Converter;
using MilkyWare.Sarif.Converter.Commands;
using MilkyWare.Sarif.Converter.Interceptors;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddLogging(builder =>
{
    builder.AddSerilog(new LoggerConfiguration()
        .MinimumLevel.ControlledBy(LoggingInterceptor.LogLevel)
        .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
        .CreateLogger());
});
services.AddTransient<ICommandInterceptor, LoggingInterceptor>();
var registrar = new TypeRegistrar(services);
var app = new CommandApp<ConvertSarifCommand>(registrar);
app.Configure(configure =>
{
    configure.SetApplicationName("milkyware-sarif-converter");

#if DEBUG
    configure.PropagateExceptions();
    configure.ValidateExamples();
#endif
});

app.Run(args);
