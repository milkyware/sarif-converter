using System.ComponentModel;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class LoggingSettings : CommandSettings
    {
        [CommandOption("--debug")]
        [Description("Increase logging verbosity to show all debug logs.")]
        [DefaultValue(false)]
        public bool Debug { get; set; }
    }
}
