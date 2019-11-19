using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

using Serilog.Sinks.LINQPad.Themes;


namespace Serilog.Sinks.LINQPad
{

    internal class ThemedHtmlWriter : TextWriter
    {
        public ThemedHtmlWriter(ConsoleTheme theme)
            : this(theme, new StringBuilder(), CultureInfo.CurrentCulture) { }


        public ThemedHtmlWriter(ConsoleTheme theme, IFormatProvider formatProvider)
            : this(theme, new StringBuilder(), formatProvider) { }


        public ThemedHtmlWriter(ConsoleTheme theme, StringBuilder sb)
            : this(theme, sb, CultureInfo.CurrentCulture) { }


        public ThemedHtmlWriter(ConsoleTheme theme, StringBuilder sb, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            _writer = new StringWriter(sb, formatProvider);
            _theme = theme;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _writer?.Dispose();
            }
            base.Dispose(disposing);
        }


        public override void Write(char value)
            => Write(value.ToString());


        public override void Write(string value)
        {
            _theme.ApplyColors(_writer);
            _writer.Write(WebUtility.HtmlEncode(value));
        }


        public override string ToString()
        {
            _theme.ApplyColors(_writer);
            return _writer.ToString();
        }


        public override Encoding Encoding => _writer.Encoding;


        private readonly ConsoleTheme _theme;
        private readonly StringWriter _writer;
    }

}
