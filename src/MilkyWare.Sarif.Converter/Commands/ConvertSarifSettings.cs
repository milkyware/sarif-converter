using System.ComponentModel;
using MilkyWare.Sarif.Converter.Enums;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class ConvertSarifSettings : LoggingSettings
    {
        [CommandOption("-f|--format")]
        [Description("Format to convert to")]
        public FormatType FormatType { get; set; } = FormatType.JUnit;

        [CommandOption("-i|--input-file")]
        [Description("Path to the input SARIF file")]
        public string? InputFile { get; set; }

        [CommandOption("-o|--output-file")]
        [Description("Path to output the converted file to. Outputs to stdout if not specified")]
        public string? OutputFile { get; set; }
    }
}
