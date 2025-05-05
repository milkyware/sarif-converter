using MilkyWare.Sarif.Converter.Enums;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class ConvertSarifSettings : LoggingSettings
    {
        [CommandOption("-f|--file")]
        public string? File { get; set; }

        [CommandOption("-t")]
        public OutputType Type { get; set; }
    }
}
