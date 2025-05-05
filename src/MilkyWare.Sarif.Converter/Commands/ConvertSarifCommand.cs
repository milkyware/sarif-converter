using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class ConvertSarifCommand(ILogger<ConvertSarifCommand> logger, IAnsiConsole ansiConsole) : AsyncCommand<ConvertSarifSettings>
    {
        public override Task<int> ExecuteAsync(CommandContext context, ConvertSarifSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
