using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;


namespace Serilog.Sinks.LINQPad
{

	internal class ColoredStringWriter : StringWriter
	{

		public ColoredStringWriter() : this(new StringBuilder(), CultureInfo.CurrentCulture) { }
		public ColoredStringWriter(IFormatProvider formatProvider) : this(new StringBuilder(), formatProvider) { }
		public ColoredStringWriter(StringBuilder sb) : this(sb, CultureInfo.CurrentCulture) { }
		public ColoredStringWriter(StringBuilder sb, IFormatProvider formatProvider) : base(formatProvider) { }


		public Color? ForegroundColor
		{
			get => _currForegroundColor;
			set => _nextForegroundColor = value;
		}


		public Color? BackgroundColor
		{
			get => _currBackgroundColor;
			set => _nextBackgroundColor = value;
		}


		public override void Write(char value)
		{
			UpdateColors();
			base.Write(SecurityElement.Escape(value.ToString()));
		}


		public override void Write(string value)
		{
			if (value.AsQueryable().All(Char.IsWhiteSpace)) {
				base.Write(value);
			} else {
				UpdateColors();
				base.Write(SecurityElement.Escape(value));
			}
		}


		public override void WriteLine(string value)
		{
			UpdateColors();
			base.WriteLine(SecurityElement.Escape(value));
		}


		public void ResetColor()
		{
			if (isOpen) {
				base.Write("</span>");
				_nextForegroundColor = null;
				_nextBackgroundColor = null;
				isOpen = false;
			}
		}


		private void UpdateColors()
		{
			if (!isOpen || _currForegroundColor != _nextForegroundColor || _currBackgroundColor != _nextBackgroundColor) {
				if (isOpen) {
					base.Write("</span>");
				}

				var declarations = new[] {
					_nextForegroundColor.HasValue ? $"color:{ColorTranslator.ToHtml(_nextForegroundColor.Value)}" : null,
					_nextBackgroundColor.HasValue ? $"background-color:{ColorTranslator.ToHtml(_nextBackgroundColor.Value)}" : null
				};
				var style = String.Join("; ", declarations.Where(x => x != null));
				if (style.Length > 0) {
					base.Write($"<span style='{style}'>");
					_currForegroundColor = _nextForegroundColor;
					_currBackgroundColor = _nextBackgroundColor;
					isOpen = true;
				}
			}
		}


		private bool isOpen = false;

		private Color? _nextForegroundColor;
		private Color? _nextBackgroundColor;

		private Color? _currForegroundColor;
		private Color? _currBackgroundColor;
	}

}
