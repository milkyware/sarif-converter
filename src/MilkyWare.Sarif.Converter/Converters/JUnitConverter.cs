using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Logging;
using MilkyWare.Sarif.Converter.Enums;

namespace MilkyWare.Sarif.Converter.Converters
{
    public class JUnitConverter(ILogger<JUnitConverter> logger) : ISarifConverter
    {
        private readonly ILogger<JUnitConverter> _logger = logger;

        public FormatType FormatType => FormatType.JUnit;

        public async Task<string> ConvertAsync(SarifLog sarif)
        {
            _logger.LogInformation("Converting SARIF to JUnit");
            var testSuite = new XElement("test-suite");
            var resultsCount = sarif.Runs.SelectMany(r => r.Results)
                .Count();
            var testSuites = new XElement("test-suites", testSuite,
                new XAttribute("tests", resultsCount),
                new XAttribute("failures", resultsCount));

            foreach (var run in sarif.Runs)
            {
                foreach (var result in run.Results)
                {
                    var location = result.Locations[0].PhysicalLocation;

                    var testCase = new XElement("testcase",
                        new XAttribute("classname", result.RuleId),
                        new XAttribute("file", location.ArtifactLocation.Uri.LocalPath),
                        new XAttribute("line", $"{location.Region.StartLine}:{location.Region.CharOffset}"));

                    testCase.Add(new XElement("failure", new XAttribute("message", result.Message.Text)));

                    testSuite.Add(testCase);
                }
            }

            using var ms = new MemoryStream();
            using var xw = XmlWriter.Create(ms, new()
            {
                Async = true,
                OmitXmlDeclaration = true,
                Indent = true
            });
            await testSuites.SaveAsync(xw, default);
            await xw.FlushAsync();

            ms.Position = 0;
            var sr = new StreamReader(ms, Encoding.UTF8);
            var xml = await sr.ReadToEndAsync();
            return xml;
        }
    }
}
