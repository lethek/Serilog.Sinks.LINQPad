using System.IO;
using Serilog.Events;
using Serilog.Sinks.LINQPad.Themes;
using Xunit;

namespace Serilog.Sinks.LINQPad.Formatting
{
    public class ThemedDisplayValueFormatterTests
    {
        [Theory]
        [InlineData("Hello", null, "\"Hello\"")]
        [InlineData("Hello", "l", "Hello")]
        public void StringFormattingIsApplied(string value, string format, string expected)
        {
            var formatter = new ThemedDisplayValueFormatter(DefaultThemes.None, null);
            var sw = new StringWriter();
            formatter.FormatLiteralValue(new ScalarValue(value), sw, format);
            var actual = sw.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
