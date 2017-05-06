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
using System.IO;

using LINQPad;

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Parsing;
using System.Security;

namespace Serilog.Sinks.LINQPad
{

	/// <summary>
	/// Writes colored log events to the LINQPad Results panel.
	/// </summary>
	public class LINQPadSink : ILogEventSink
	{

		/// <summary>
		/// Construct a sink that writes colored log events to LINQPad's Results panel.
		/// </summary>
		/// <param name="outputTemplate">A message template describing the format used to write to the sink.</param>
		/// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
		public LINQPadSink(string outputTemplate, IFormatProvider formatProvider)
		{
			if (outputTemplate == null) {
				throw new ArgumentNullException("outputTemplate");
			}
			_outputTemplate = new MessageTemplateParser().Parse(outputTemplate);
			_formatProvider = formatProvider;
		}

		/// <summary>
		/// Emit the provided log event to the sink.
		/// </summary>
		/// <param name="logEvent">The log event to write.</param>
		public void Emit(LogEvent logEvent)
		{
			if (logEvent == null) {
				throw new ArgumentNullException("logEvent");
			}

			var outputProperties = OutputProperties.GetOutputProperties(logEvent);
			var palette = GetPalette(logEvent.Level);
			var state = new ConsoleColorState();

			using (var output = new StringWriter(_formatProvider)) {
				lock (_syncRoot) {
					try {
						foreach (var outputToken in _outputTemplate.Tokens) {
							var propertyToken = outputToken as PropertyToken;
							if (propertyToken == null) {
								RenderOutputToken(palette, outputToken, outputProperties, output, state);
							} else {
								switch (propertyToken.PropertyName) {
									case OutputProperties.MessagePropertyName:
										RenderMessageToken(logEvent, palette, output, state);
										break;
									case OutputProperties.ExceptionPropertyName:
										RenderExceptionToken(palette, propertyToken, outputProperties, output, state);
										break;
									default:
										RenderOutputToken(palette, outputToken, outputProperties, output, state);
										break;
								}
							}
						}
					} finally {
						if (state.Colors != null) {
							CloseColors(output, state);
						}
						Util.RawHtml($"<span style='white-space:pre-wrap'>{output.ToString()}</span>").Dump();
					}
				}
			}
		}

		private void RenderExceptionToken(Palette palette, MessageTemplateToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, TextWriter output, ConsoleColorState state)
		{
			using (var sw = new StringWriter()) {
				outputToken.Render(outputProperties, sw, _formatProvider);
				var lines = new StringReader(sw.ToString());
				string nextLine;
				while ((nextLine = lines.ReadLine()) != null) {
					var newColors = nextLine.StartsWith(StackFrameLinePrefix)
						? palette.Base
						: palette.Highlight;

					if (AreColorsDifferent(state.Colors, newColors)) {
						if (state.Colors.HasValue) {
							CloseColors(output, state);
						}
						StartColors(output, state, newColors);
					}

					output.WriteLine(SecurityElement.Escape(nextLine));
				}
			}
		}

		private void RenderMessageToken(LogEvent logEvent, Palette palette, TextWriter output, ConsoleColorState state)
		{
			foreach (var messageToken in logEvent.MessageTemplate.Tokens) {
				var newColors = (messageToken as PropertyToken) != null
					? palette.Highlight
					: palette.Base;

				if (AreColorsDifferent(state.Colors, newColors)) {
					if (state.Colors.HasValue) {
						CloseColors(output, state);
					}
					StartColors(output, state, newColors);
				}

				using (var writer = new StringWriter()) {
					messageToken.Render(logEvent.Properties, writer, _formatProvider);
					output.Write(SecurityElement.Escape(writer.ToString()));
				}
			}
		}

		private void RenderOutputToken(Palette palette, MessageTemplateToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, TextWriter output, ConsoleColorState state)
		{
			if (AreColorsDifferent(state.Colors, palette.Base)) {
				if (state.Colors.HasValue) {
					CloseColors(output, state);
				}
				StartColors(output, state, palette.Base);
			}
			using (var writer = new StringWriter()) {
				outputToken.Render(outputProperties, writer, _formatProvider);
				output.Write(SecurityElement.Escape(writer.ToString()));
			}
		}

		private static Palette GetPalette(LogEventLevel level)
		{
			Palette palette;
			if (!LevelPalettes.TryGetValue(level, out palette)) {
				palette = DefaultPalette;
			}
			return palette;
		}

		private static bool AreColorsDifferent(ConsoleColorPair? current, ConsoleColorPair? next)
			=> current?.Background != next?.Background || current?.Foreground != next?.Foreground;


		private static void StartColors(TextWriter output, ConsoleColorState state, ConsoleColorPair newColors)
		{
			output.Write($"<span style='color:{newColors.Foreground}; background-color:{newColors.Background}'>");
			state.Colors = newColors;
		}

		private static void CloseColors(TextWriter output, ConsoleColorState state)
		{
			output.Write("</span>");
			state.Colors = null;
		}


		private readonly IFormatProvider _formatProvider;

		private static readonly Palette DefaultPalette = new Palette {
			Base = new ConsoleColorPair { Background = ConsoleColor.Black, Foreground = ConsoleColor.Gray },
			Highlight = new ConsoleColorPair { Background = ConsoleColor.DarkGray, Foreground = ConsoleColor.Gray }
		};


		private static readonly IDictionary<LogEventLevel, Palette> LevelPalettes = new Dictionary<LogEventLevel, Palette>
		{
			{ LogEventLevel.Verbose,     new Palette { Base = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.DarkGray },
													   Highlight = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.Gray } } },
			{ LogEventLevel.Debug,       new Palette { Base = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.Gray },
													   Highlight = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.Black } } },
			{ LogEventLevel.Information, new Palette { Base = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.Black },
													   Highlight = new ConsoleColorPair { Background = ConsoleColor.DarkBlue, Foreground = ConsoleColor.White } } },
			{ LogEventLevel.Warning,     new Palette { Base = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.Yellow },
													   Highlight = new ConsoleColorPair { Background = ConsoleColor.DarkYellow, Foreground = ConsoleColor.Black } } },
			{ LogEventLevel.Error,       new Palette { Base = new ConsoleColorPair { Background = ConsoleColor.White, Foreground = ConsoleColor.Red },
													   Highlight = new ConsoleColorPair { Background = ConsoleColor.Red, Foreground = ConsoleColor.White } } },
			{ LogEventLevel.Fatal,       new Palette { Base = new ConsoleColorPair { Background = ConsoleColor.DarkRed, Foreground = ConsoleColor.Black },
													   Highlight = new ConsoleColorPair { Background = ConsoleColor.Red, Foreground = ConsoleColor.White } } }
		};

		private readonly object _syncRoot = new object();
		private readonly MessageTemplate _outputTemplate;

		private const string StackFrameLinePrefix = "   ";

		private struct ConsoleColorPair
		{
			public ConsoleColor Foreground { get; set; }
			public ConsoleColor Background { get; set; }
		}

		private class ConsoleColorState
		{
			public ConsoleColorPair? Colors { get; set; }
		}

		private struct Palette
		{
			public ConsoleColorPair Base { get; set; }
			public ConsoleColorPair Highlight { get; set; }
		}

	}

}
