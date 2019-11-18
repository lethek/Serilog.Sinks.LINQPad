using System;

using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.LINQPad.Output;
using Serilog.Sinks.LINQPad.Themes;

using Xunit;

namespace Serilog.Sinks.LINQPad
{
    public class LINQPadSinkTests
    {
        [Fact]
        public void Test1()
        {
            var theme = ConsoleThemes.LiterateDark;
            var formatter = new OutputTemplateRenderer(theme, DefaultConsoleOutputTemplate, null);
            var sink = new LINQPadSink(theme, formatter, LogEventLevel.Verbose);

            var msg = new MessageTemplate("Hello World", new MessageTemplateToken[0]);
            var evt = new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Debug, null, msg, new LogEventProperty[0]);
            sink.Emit(evt);
        }

        const string DefaultConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
    }
}
