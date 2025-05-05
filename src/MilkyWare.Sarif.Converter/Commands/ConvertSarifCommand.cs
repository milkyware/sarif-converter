using System.Xml.Linq;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Commands
{
    public class ConvertSarifCommand(ILogger<ConvertSarifCommand> logger, IAnsiConsole ansiConsole) : AsyncCommand<ConvertSarifSettings>
    {
        private readonly IAnsiConsole _ansiConsole = ansiConsole;
        private readonly ILogger<ConvertSarifCommand> _logger = logger;

        public override async Task<int> ExecuteAsync(CommandContext context, ConvertSarifSettings settings)
        {
            var sarif = SarifLog.Load(settings.File);

            var xml = ConvertToNUnit(sarif);
            _ansiConsole.Write(xml);
            return 0;
        }

        #region Private Helpers

        private string ConvertToNUnit(SarifLog sarif)
        {
            var testCases = new List<XElement>();
            foreach (var run in sarif.Runs)
            {
                foreach (var result in run.Results)
                {
                    var testCase = new XElement("test-case",
                        new XAttribute("name", result.RuleId),
                        new XAttribute("result", "Failed"));

                    var properties = new List<XElement>();
                    if (!string.IsNullOrWhiteSpace(result.Message.Text))
                    {
                        properties.Add(new XElement("property",
                            new XAttribute("name", "Description"),
                            new XAttribute("value", result.Message.Text)));
                    }

                    if (properties.Any())
                    {
                        testCases.Add(new XElement("properties",
                            new XElement("property", properties)));
                    }
                    testCases.Add(testCase);
                }
            }
            var testSuite = new XElement("test-suite");
            testSuite.Add(testCases);

            var testResults = new XElement("test-results");
            testResults.SetAttributeValue("result", "Failed");
            testResults.SetAttributeValue("total", testCases.Count);
            testResults.SetAttributeValue("failed", testCases.Count);
            testResults.Add(testSuite);

            return testResults.ToString();
        }

        #endregion Private Helpers
    }
}
