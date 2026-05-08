using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MilkyWare.Sarif.Converter;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using Spectre.Console.Testing;

namespace MilkyWare.Sarif.ConverterTests
{
    internal static class CommandAppTestHarness
    {
        public static CommandAppTester Create<TCommand>(string commandName, Action<IServiceCollection> configureServices)
            where TCommand : class, ICommand
        {
            var console = new TestConsole().Width(int.MaxValue);
            var services = new ServiceCollection();

            services.AddSingleton<IAnsiConsole>(console);
            configureServices(services);

            var app = new CommandAppTester(new TypeRegistrar(services), console: console);
            app.Configure(configure =>
            {
                configure.AddCommand<TCommand>(commandName);
            });

            return app;
        }
    }
}
