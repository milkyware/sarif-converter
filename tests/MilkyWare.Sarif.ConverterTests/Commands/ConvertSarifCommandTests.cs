using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MilkyWare.Sarif.Converter.Converters;
using MilkyWare.Sarif.Converter.Enums;
using MilkyWare.Sarif.ConverterTests;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;

namespace MilkyWare.Sarif.Converter.Commands.Tests
{
    public class ConvertSarifCommandTests
    {
        private readonly CommandAppTester _app;
        private readonly ISarifConverter _converter = Substitute.For<ISarifConverter>();
        private readonly ILogger<ConvertSarifCommand> _logger = Substitute.For<ILogger<ConvertSarifCommand>>();

        public ConvertSarifCommandTests()
        {
            _converter.ConvertAsync(Arg.Any<SarifLog>())
                .Returns("<results />");

            _app = CommandAppTestHarness.Create<ConvertSarifCommand>("convert-sarif", services =>
            {
                services.AddSingleton(_logger);
                services.AddSingleton(_converter);
            });
        }

        [Fact()]
        public async Task ExecuteAsyncTest_WhenConverterFound_ConvertsSarif()
        {
            // Arrange
            var inputFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(inputFile, """
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
                                  "uri": "file:///D:/Git/milkyware/azure-bicep/./src/storageaccount.bicep"
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
                                  "uri": "file:///D:/Git/milkyware/azure-bicep/./src/storageaccount.bicep"
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

            var remainingArgs = Substitute.For<IRemainingArguments>();
            var context = new CommandContext([], remainingArgs, "dummy", null);
            var settings = new ConvertSarifSettings
            {
                InputFile = inputFile,
                FormatType = FormatType.JUnit
            };

            CommandAppResult actual;
            try
            {
                // Act
                actual = await _app.RunAsync(["convert-sarif", "--input-file", inputFile, "--format-type", "JUnit"]);
            }
            finally
            {
                File.Delete(inputFile);
            }

            // Assert
            actual.ExitCode.Should()
                .Be(0);
            await _converter.Received(1)
                .ConvertAsync(Arg.Any<SarifLog>());
            _app.Console.Output.Should()
                .NotBeNullOrWhiteSpace();
        }

        [Fact()]
        public async Task ExecuteAsyncTest_WhenConverterFound_ShouldConvertsSarif_AndWriteToFile()
        {
            // Arrange
            var inputFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(inputFile, """
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
                                  "uri": "file:///D:/Git/milkyware/azure-bicep/./src/storageaccount.bicep"
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
                                  "uri": "file:///D:/Git/milkyware/azure-bicep/./src/storageaccount.bicep"
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

            // Arrange Output
            var outputFile = Path.GetTempFileName();

            // Arranhe Command
            var remainingArgs = Substitute.For<IRemainingArguments>();
            var context = new CommandContext([], remainingArgs, "dummy", null);
            var settings = new ConvertSarifSettings
            {
                InputFile = inputFile,
                OutputFile = outputFile,
                FormatType = FormatType.JUnit
            };

            try
            {
                // Act
                var actual = await _app.RunAsync([
                    "convert-sarif",
                    "--input-file", inputFile,
                    "--output-file", outputFile,
                    "--format-type", "JUnit"]);

                // Assert
                actual.ExitCode.Should()
                    .Be(0);
                await _converter.Received(1)
                    .ConvertAsync(Arg.Any<SarifLog>());
                new FileInfo(outputFile).Length.Should()
                    .BeGreaterThan(0);
            }
            finally
            {
                File.Delete(inputFile);
                File.Delete(outputFile);
            }
        }

        [Fact]
        public async Task ExecuteAsyncTest_WhenFormatTypeUnsupported()
        {
            // Arrange
            var inputFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(inputFile, """
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
                                  "uri": "file:///D:/Git/milkyware/azure-bicep/./src/storageaccount.bicep"
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
                                  "uri": "file:///D:/Git/milkyware/azure-bicep/./src/storageaccount.bicep"
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

            _converter.FormatType.Returns(FormatType.NUnit);

            var remainingArgs = Substitute.For<IRemainingArguments>();
            var context = new CommandContext([], remainingArgs, "dummy", null);
            var settings = new ConvertSarifSettings
            {
                InputFile = inputFile,
                FormatType = FormatType.JUnit
            };

            CommandAppResult actual;
            try
            {
                // Act
                actual = await _app.RunAsync(["convert-sarif", "--input-file", inputFile, "--format-type", "JUnit"]);
            }
            finally
            {
                File.Delete(inputFile);
            }

            // Assert
            actual.ExitCode.Should()
                .Be(1);
        }
    }
}
