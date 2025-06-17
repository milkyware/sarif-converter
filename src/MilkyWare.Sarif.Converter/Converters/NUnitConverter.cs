using System.Xml.Linq;
using Microsoft.CodeAnalysis.Sarif;
using MilkyWare.Sarif.Converter.Enums;

namespace MilkyWare.Sarif.Converter.Converters
{
    public class NUnitConverter : ISarifConverter
    {
        public FormatType FormatType => FormatType.NUnit;

        public async Task<string> ConvertAsync(SarifLog sarif)
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
    }
}
