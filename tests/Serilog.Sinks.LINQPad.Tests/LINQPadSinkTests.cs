using System;
using System.Linq;

using LP = LINQPad;

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
        public void EmitToOutputHasNoExceptions()
        {
            var formatter = new OutputTemplateRenderer(DefaultTestTheme, DefaultConsoleOutputTemplate, null);
            var sink = new LINQPadSink(DefaultTestTheme, formatter);

            sink.Emit(CreateLogEvent("Hello LINQPad"));

            //I can't find any way to assert that content was really written to LINQPad's results output panel, so this test
            //merely ensures that no exceptions are thrown instead
        }


        [Fact]
        public void EmitToDumpContainerChangesContent()
        {
            var contentChanged = false;
            var dcLogger = new LP.DumpContainer();
            dcLogger.ContentChanged += (s, e) => contentChanged = true;

            var formatter = new OutputTemplateRenderer(DefaultTestTheme, DefaultConsoleOutputTemplate, null);
            var sink = new LINQPadSink(DefaultTestTheme, formatter, dcLogger);

            sink.Emit(CreateLogEvent("Hello LINQPad"));

            Assert.True(contentChanged, "Content in the DumpContainer should have changed.");
        }


        private static LogEvent CreateLogEvent(string text)
        {
            var msg = new MessageTemplate(text, Enumerable.Empty<MessageTemplateToken>());
            return new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Debug, null, msg, Enumerable.Empty<LogEventProperty>());
        }


        
        private const string DefaultConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        private static readonly LINQPadTheme DefaultTestTheme = DefaultThemes.LINQPadLiterate;
    }
}
