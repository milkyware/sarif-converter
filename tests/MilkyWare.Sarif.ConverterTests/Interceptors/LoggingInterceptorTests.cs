using MilkyWare.Sarif.Converter.Commands;
using Serilog.Events;
using Spectre.Console.Cli;

namespace MilkyWare.Sarif.Converter.Interceptors.Tests
{
    public class LoggingInterceptorTests : IDisposable
    {
        private readonly LoggingInterceptor _interceptor;

        public LoggingInterceptorTests()
        {
            _interceptor = new LoggingInterceptor();
        }

        public void Dispose()
        {
            LoggingInterceptor.LogLevel.MinimumLevel = LogEventLevel.Warning;
        }

        [Fact()]
        public void InterceptTest_WhenDebugSettingFalse_ShouldHaveWarningLogLevel()
        {
            // Arrange
            var remainingArgs = Substitute.For<IRemainingArguments>();
            var context = new CommandContext([], remainingArgs, "dummy", null);
            var settings = new LoggingSettings
            {
                Debug = false
            };

            // Act
            _interceptor.Intercept(context, settings);

            // Assert
            LoggingInterceptor.LogLevel.MinimumLevel.Should()
                .Be(LogEventLevel.Warning);
        }

        [Fact()]
        public void InterceptTest_WhenDebugSettingTrue_ShouldHaveVerboseLogLevel()
        {
            // Arrange
            var remainingArgs = Substitute.For<IRemainingArguments>();
            var context = new CommandContext([], remainingArgs, "dummy", null);
            var settings = new LoggingSettings
            {
                Debug = true
            };

            // Act
            _interceptor.Intercept(context, settings);

            // Assert
            LoggingInterceptor.LogLevel.MinimumLevel.Should()
                .Be(LogEventLevel.Verbose);
        }
    }
}
