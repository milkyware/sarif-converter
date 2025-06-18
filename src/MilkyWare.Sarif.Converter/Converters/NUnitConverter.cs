using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Logging;
using MilkyWare.Sarif.Converter.Enums;

namespace MilkyWare.Sarif.Converter.Converters
{
    public class NUnitConverter(ILogger<NUnitConverter> logger) : ISarifConverter
    {
        private readonly ILogger<NUnitConverter> _logger = logger;

        public FormatType FormatType => FormatType.NUnit;

        public async Task<string> ConvertAsync(SarifLog sarif)
        {
            var resultsCount = sarif.Runs.SelectMany(r => r.Results)
                .Count();
            var testRun = new XElement("test-run",
                new XAttribute("testcasecount", resultsCount),
                new XAttribute("total", resultsCount),
                new XAttribute("failed", resultsCount),
                new XAttribute("result", "Failed"));

            var testSuite = new XElement("test-suite");
            testRun.Add(testSuite);

            foreach (var run in sarif.Runs)
            {
                foreach (var result in run.Results)
                {
                    var testCase = new XElement("test-case",
                        new XAttribute("name", result.Message.Text),
                        new XAttribute("classname", result.RuleId),
                        new XAttribute("result", "Failed"));

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
            await testRun.SaveAsync(xw, default);
            await xw.FlushAsync();

            ms.Position = 0;
            var sr = new StreamReader(ms, Encoding.UTF8);
            var xml = await sr.ReadToEndAsync();
            return xml;
        }
    }
}
