// Copyright 2015 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

using LINQPad;

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Parsing;

namespace Serilog.Sinks.LINQPad
{

	/// <summary>
	/// Writes colored log events to the LINQPad Results panel.
	/// </summary>
	public class LINQPadSink : ILogEventSink
	{

		/// <summary>
		/// Defines the default color-palette used by this sink, by default it uses colors chosen for use on white backgrounds
		/// </summary>
		public static readonly OutputPalette DefaultPalette = CreateLightPalette();


		/// <summary>
		/// Creates a new OutputPalette for use on white/light backgrounds
		/// </summary>
		/// <returns>New OutputPalette instance</returns>
		public static OutputPalette CreateLightPalette()
			=> new OutputPalette();


		/// <summary>
		/// Creates a new OutputPalette for use on black/dark backgrounds
		/// </summary>
		/// <returns>New OutputPalette instance</returns>
		public static OutputPalette CreateDarkPalette()
			=> new OutputPalette {
				Text = new ColorPair(Color.White),
				InformationLevel = new ColorPair(Color.White),
				WarningLevel = new ColorPair(Color.Yellow),
				StringSymbol = new ColorPair(Color.Cyan),
				OtherSymbol = new ColorPair(Color.LawnGreen),
				RawText = new ColorPair(Color.Yellow)
			};


		/// <summary>
		/// Construct a sink that writes colored log events to LINQPad's Results panel.
		/// </summary>
		/// <param name="outputTemplate">A message template describing the format used to write to the sink.</param>
		/// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
		/// <param name="outputPalette">Supplies custom a color-palette for rendered output. If none is specified, then the default <see cref="LINQPadSink.DefaultPalette"/> is used instead.</param>
		public LINQPadSink(string outputTemplate, IFormatProvider formatProvider, OutputPalette outputPalette = null)
		{
			if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));

			_outputTemplate = new MessageTemplateParser().Parse(outputTemplate);
			_formatProvider = formatProvider;
			_palette = outputPalette ?? DefaultPalette;

			_levels = new Dictionary<LogEventLevel, Func<ColorPair>> {
				{ LogEventLevel.Verbose, () => _palette.VerboseLevel },
				{ LogEventLevel.Debug, () => _palette.DebugLevel },
				{ LogEventLevel.Information, () => _palette.InformationLevel },
				{ LogEventLevel.Warning, () => _palette.WarningLevel },
				{ LogEventLevel.Error, () => _palette.ErrorLevel },
				{ LogEventLevel.Fatal, () => _palette.FatalLevel }
			};
		}


		/// <summary>
		/// Emit the provided log event to the sink.
		/// </summary>
		/// <param name="logEvent">The log event to write.</param>
		public void Emit(LogEvent logEvent)
		{
			if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

			var outputProperties = OutputProperties.GetOutputProperties(logEvent);
			using (var outputStream = new ColoredStringWriter(_formatProvider)) {
				lock (_syncRoot) {
					try {
						foreach (var outputToken in _outputTemplate.Tokens) {
							var propertyToken = outputToken as PropertyToken;
							if (propertyToken == null) {
								RenderOutputTemplateTextToken(outputToken, outputProperties, outputStream);
							} else switch (propertyToken.PropertyName) {
									case OutputProperties.LevelPropertyName:
										RenderLevelToken(logEvent.Level, outputToken, outputProperties, outputStream);
										break;
									case OutputProperties.MessagePropertyName:
										RenderMessageToken(logEvent, outputStream);
										break;
									case OutputProperties.ExceptionPropertyName:
										RenderExceptionToken(propertyToken, outputProperties, outputStream);
										break;
									default:
										RenderOutputTemplatePropertyToken(propertyToken, outputProperties, outputStream);
										break;
								}
						}
					} finally {
						outputStream.ResetColor();
						Util.RawHtml($"<span style='white-space:pre-wrap'>{outputStream}</span>").Dump();
					}
				}
			}
		}


		private void RenderExceptionToken(PropertyToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, ColoredStringWriter outputStream)
		{
			var sw = new StringWriter();
			outputToken.Render(outputProperties, sw, _formatProvider);
			var lines = new StringReader(sw.ToString());
			string nextLine;
			while ((nextLine = lines.ReadLine()) != null) {
				outputStream.SetColors(nextLine.StartsWith(StackFrameLinePrefix) ? _palette.Subtext : _palette.Text);
				outputStream.WriteLine(nextLine);
			}
		}


		private void RenderOutputTemplatePropertyToken(PropertyToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, ColoredStringWriter outputStream)
		{
			outputStream.SetColors(_palette.Subtext);

			// This code is shared with MessageTemplateFormatter in the core Serilog
			// project. Its purpose is to modify the way tokens are formatted to
			// use "output template" rather than "message template" rules.

			// First variation from normal rendering - if a property is missing,
			// don't render anything (message templates render the raw token here).
			LogEventPropertyValue propertyValue;
			if (!outputProperties.TryGetValue(outputToken.PropertyName, out propertyValue))
				return;

			// Second variation; if the value is a scalar string, use literal
			// rendering and support some additional formats: 'u' for uppercase
			// and 'w' for lowercase.
			var sv = propertyValue as ScalarValue;
			if (sv?.Value is string) {
				var overridden = new Dictionary<string, LogEventPropertyValue> {
					{ outputToken.PropertyName, new LiteralStringValue((string) sv.Value) }
				};

				outputToken.Render(overridden, outputStream, _formatProvider);
			} else {
				outputToken.Render(outputProperties, outputStream, _formatProvider);
			}
		}


		private void RenderLevelToken(LogEventLevel level, MessageTemplateToken token, IReadOnlyDictionary<string, LogEventPropertyValue> properties, ColoredStringWriter outputStream)
		{
			if (!_levels.TryGetValue(level, out var getLevelColors))
				getLevelColors = _levels[LogEventLevel.Warning];

			outputStream.SetColors(getLevelColors());

			token.Render(properties, outputStream);
			outputStream.ResetColor();
		}


		private void RenderOutputTemplateTextToken(MessageTemplateToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, ColoredStringWriter outputStream)
		{
			outputStream.SetColors(_palette.Punctuation);
			outputToken.Render(outputProperties, outputStream, _formatProvider);
		}


		private void RenderMessageToken(LogEvent logEvent, ColoredStringWriter outputStream)
		{
			foreach (var messageToken in logEvent.MessageTemplate.Tokens) {
				var messagePropertyToken = messageToken as PropertyToken;
				if (messagePropertyToken != null) {
					LogEventPropertyValue value;
					if (!logEvent.Properties.TryGetValue(messagePropertyToken.PropertyName, out value)) {
						outputStream.SetColors(_palette.RawText);
						outputStream.Write(messagePropertyToken);
					} else {
						var scalar = value as ScalarValue;
						if (scalar != null) {
							outputStream.SetColors(GetScalarColor(scalar));

							if (scalar.Value is string && messagePropertyToken.Format == null && messagePropertyToken.Alignment == null) {
								outputStream.Write(scalar.Value);
							} else if (scalar.Value is bool && messagePropertyToken.Format == null && messagePropertyToken.Alignment == null) {
								outputStream.Write(scalar.Value.ToString().ToLowerInvariant());
							} else {
								messagePropertyToken.Render(logEvent.Properties, outputStream, _formatProvider);
							}
						} else {
							PrettyPrint(value, messagePropertyToken.Format, _formatProvider, outputStream);
						}
					}
				} else {
					outputStream.SetColors(_palette.Text);
					messageToken.Render(logEvent.Properties, outputStream, _formatProvider);
				}
			}
		}


		private void PrettyPrint(LogEventPropertyValue value, string format, IFormatProvider formatProvider, ColoredStringWriter outputStream)
		{
			var scalar = value as ScalarValue;
			if (scalar != null) {
				outputStream.SetColors(GetScalarColor(scalar));
				value.Render(outputStream, format, formatProvider);
				return;
			}

			var seq = value as SequenceValue;
			if (seq != null) {
				outputStream.SetColors(_palette.Punctuation);
				outputStream.Write("[");

				var sep = "";
				foreach (var element in seq.Elements) {
					outputStream.SetColors(_palette.Punctuation);
					outputStream.Write(sep);
					sep = ", ";

					PrettyPrint(element, null, formatProvider, outputStream);
				}

				outputStream.SetColors(_palette.Punctuation);
				outputStream.Write("]");
				return;
			}

			var str = value as StructureValue;
			if (str != null) {
				if (str.TypeTag != null) {
					outputStream.SetColors(_palette.Subtext);
					outputStream.Write(str.TypeTag);
					outputStream.Write(" ");
				}

				outputStream.SetColors(_palette.Punctuation);
				outputStream.Write("{");

				var sep = "";
				foreach (var prop in str.Properties) {
					outputStream.SetColors(_palette.Punctuation);
					outputStream.Write(sep);
					sep = ", ";

					outputStream.SetColors(_palette.NameSymbol);
					outputStream.Write(prop.Name);

					outputStream.SetColors(_palette.Punctuation);
					outputStream.Write("=");

					PrettyPrint(prop.Value, null, formatProvider, outputStream);
				}

				outputStream.SetColors(_palette.Punctuation);
				outputStream.Write("}");
				return;
			}

			var div = value as DictionaryValue;
			if (div != null) {
				outputStream.SetColors(_palette.Punctuation);
				outputStream.Write("{");

				var sep = "";
				foreach (var element in div.Elements) {
					outputStream.SetColors(_palette.Punctuation);
					outputStream.Write(sep);
					sep = ", ";
					outputStream.Write("[");
					PrettyPrint(element.Key, null, formatProvider, outputStream);

					outputStream.SetColors(_palette.Punctuation);
					outputStream.Write("]=");

					PrettyPrint(element.Value, null, formatProvider, outputStream);
				}

				outputStream.SetColors(_palette.Punctuation);
				outputStream.Write("}");
				return;
			}

			value.Render(outputStream, format, formatProvider);
		}


		private ColorPair GetScalarColor(ScalarValue scalar)
		{
			if (scalar.Value == null || scalar.Value is bool)
				return _palette.KeywordSymbol;

			if (scalar.Value is string)
				return _palette.StringSymbol;

			if (scalar.Value.GetType().GetTypeInfo().IsPrimitive || scalar.Value is decimal)
				return _palette.NumericSymbol;

			return _palette.OtherSymbol;
		}


		private const string StackFrameLinePrefix = "   ";

		private readonly OutputPalette _palette;
		private readonly IFormatProvider _formatProvider;
		private readonly MessageTemplate _outputTemplate;
		private readonly object _syncRoot = new Object();

		private readonly IDictionary<LogEventLevel, Func<ColorPair>> _levels;

	}

}
