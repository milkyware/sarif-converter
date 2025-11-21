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

        public override async Task<int> ExecuteAsync(CommandContext context, ConvertSarifSettings settings, CancellationToken cancellationToken)
        {
            var sarif = SarifLog.Load(settings.InputFile);

            var converter = _converters.FirstOrDefault(c => c.FormatType == settings.FormatType);
            if (converter == null)
            {
                _logger.LogError("Unsupported output type");
                return 1;
            }

            var xml = await converter.ConvertAsync(sarif);

            if (string.IsNullOrEmpty(settings.OutputFile))
            {
                _ansiConsole.Write(xml);
                return 0;
            }

            var directory = Path.GetDirectoryName(settings.OutputFile);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(settings.OutputFile, xml);
            return 0;
        }
    }
}
