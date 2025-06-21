using MilkyWare.Sarif.Converter.Enums;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class ConvertSarifSettings : LoggingSettings
    {
        [CommandOption("-f|--format")]
        public FormatType FormatType { get; set; } = FormatType.JUnit;

        [CommandOption("-i|--input-file")]
        public string? InputFile { get; set; }

        [CommandOption("-o|--output-file")]
        public string? OutputFile { get; set; }
    }
}
