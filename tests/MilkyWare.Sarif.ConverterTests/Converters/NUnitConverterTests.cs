using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Logging;

namespace MilkyWare.Sarif.Converter.Converters.Tests
{
    public class NUnitConverterTests
    {
        private readonly NUnitConverter _converter;
        private readonly ILogger<NUnitConverter> _logger;

        public NUnitConverterTests()
        {
            _logger = Substitute.For<ILogger<NUnitConverter>>();
            _converter = new NUnitConverter(_logger);
        }

        [Fact()]
        public async Task ConvertAsyncTest()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms);
            await sw.WriteAsync("""
                                {
                    "$schema": "https://schemastore.azurewebsites.net/schemas/json/sarif-2.1.0-rtm.6.json",
                    "version": "2.1.0",
                    "runs": [
                        {
                            "tool": {
                                "driver": {
                                    "name": "bicep"
                                }
                            },
                            "results": [
                                {
                                    "ruleId": "outputs-should-not-contain-secrets",
                                    "message": {
                                        "text": "Outputs should not contain secrets. Found possible secret: function 'listKeys' [https://aka.ms/bicep/linter/outputs-should-not-contain-secrets]"
                                    },
                                    "locations": [
                                        {
                                            "physicalLocation": {
                                                "artifactLocation": {
                                                    "uri": "file:///C:/Git/milkyware/azure-bicep/./.tmp/storageaccount.bicep"
                                                },
                                                "region": {
                                                    "startLine": 206,
                                                    "charOffset": 113
                                                }
                                            }
                                        }
                                    ]
                                },
                                {
                                    "ruleId": "outputs-should-not-contain-secrets",
                                    "message": {
                                        "text": "Outputs should not contain secrets. Found possible secret: function 'listKeys' [https://aka.ms/bicep/linter/outputs-should-not-contain-secrets]"
                                    },
                                    "locations": [
                                        {
                                            "physicalLocation": {
                                                "artifactLocation": {
                                                    "uri": "file:///C:/Git/milkyware/azure-bicep/./.tmp/storageaccount.bicep"
                                                },
                                                "region": {
                                                    "startLine": 207,
                                                    "charOffset": 35
                                                }
                                            }
                                        }
                                    ]
                                }
                            ],
                            "columnKind": "utf16CodeUnits"
                        }
                    ]
                }
                """);
            await sw.FlushAsync();
            ms.Position = 0;
            var input = SarifLog.Load(ms);

            // Act
            var actual = await _converter.ConvertAsync(input);

            // Assert
            Xunit.Assert.Fail("This test needs an implementation");
        }
    }
}
