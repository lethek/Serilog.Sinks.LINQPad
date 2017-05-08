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
			get => _currColors.Foreground;
			set => _nextColors.Foreground = value;
		}


		public Color? BackgroundColor
		{
			get => _currColors.Background;
			set => _nextColors.Background = value;
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
			if (_isOpen) {
				base.Write("</span>");
				_nextColors.Foreground = null;
				_nextColors.Background = null;
				_isOpen = false;
			}
		}


		public void SetColors(ColorPair colors)
		{
			_nextColors.Foreground = colors.Foreground;
			_nextColors.Background = colors.Background;
		}


		private void UpdateColors()
		{
			if (!_isOpen || _currColors.Foreground != _nextColors.Foreground || _currColors.Background != _nextColors.Background) {
				if (_isOpen) {
					base.Write("</span>");
				}

				var declarations = new[] {
					_nextColors.Foreground.HasValue ? $"color:{ColorTranslator.ToHtml(_nextColors.Foreground.Value)}" : null,
					_nextColors.Background.HasValue ? $"background-color:{ColorTranslator.ToHtml(_nextColors.Background.Value)}" : null
				};
				var style = String.Join("; ", declarations.Where(x => x != null));
				if (style.Length > 0) {
					base.Write($"<span style='{style}'>");
					_currColors.Foreground = _nextColors.Foreground;
					_currColors.Background = _nextColors.Background;
					_isOpen = true;
				}
			}
		}


		private bool _isOpen = false;
		
		private readonly ColorPair _nextColors = new ColorPair();
		private readonly ColorPair _currColors = new ColorPair();

	}

}
