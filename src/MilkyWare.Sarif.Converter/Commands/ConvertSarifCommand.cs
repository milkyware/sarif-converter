using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Logging;
using MilkyWare.Sarif.Converter.Converters;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class ConvertSarifCommand(ILogger<ConvertSarifCommand> logger, IAnsiConsole ansiConsole, IEnumerable<ISarifConverter> converters) : AsyncCommand<ConvertSarifSettings>
    {
        private readonly IAnsiConsole _ansiConsole = ansiConsole;
        private readonly IEnumerable<ISarifConverter> _converters = converters;
        private readonly ILogger<ConvertSarifCommand> _logger = logger;

        public override async Task<int> ExecuteAsync(CommandContext context, ConvertSarifSettings settings)
        {
            var sarif = SarifLog.Load(settings.File);

            var converter = _converters.FirstOrDefault(c => c.OutputType == settings.OutputType);
            if (converter == null)
            {
                _logger.LogError("Unsupported output type");
                return 1;
            }

            var xml = await converter.ConvertAsync(sarif);
            _ansiConsole.Write(xml);
            return 0;
        }
    }
}
