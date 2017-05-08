using System;
using System.IO;

using Serilog.Events;


namespace Serilog.Sinks.LINQPad
{

	internal class LiteralStringValue : LogEventPropertyValue
	{

		public LiteralStringValue(string value)
		{
			_value = value ?? throw new ArgumentNullException(nameof(value));
		}


		public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
		{
			var toRender = _value;

			switch (format) {
				case "u":
					toRender = _value.ToUpperInvariant();
					break;
				case "w":
					toRender = _value.ToLowerInvariant();
					break;
			}

			output.Write(toRender);
		}


		public override bool Equals(object obj)
		{
			var sv = obj as LiteralStringValue;
			return sv != null && Equals(_value, sv._value);
		}


		public override int GetHashCode()
			=> _value.GetHashCode();


		private readonly string _value;

	}
}
