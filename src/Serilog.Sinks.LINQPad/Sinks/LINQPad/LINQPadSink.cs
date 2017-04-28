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
			using (var output = new StringWriter(_formatProvider)) {
				lock (_syncRoot) {
					try {
						foreach (var outputToken in _outputTemplate.Tokens) {
							var propertyToken = outputToken as PropertyToken;
							if (propertyToken == null) {
								RenderOutputToken(palette, outputToken, outputProperties, output);
							} else {
								switch (propertyToken.PropertyName) {
									case OutputProperties.MessagePropertyName:
										RenderMessageToken(logEvent, palette, output);
										break;
									case OutputProperties.ExceptionPropertyName:
										RenderExceptionToken(palette, propertyToken, outputProperties, output);
										break;
									default:
										RenderOutputToken(palette, outputToken, outputProperties, output);
										break;
								}
							}
						}
					} finally {
						Util.RawHtml(output.ToString()).Dump();
					}
				}
			}
		}

		private void RenderExceptionToken(Palette palette, MessageTemplateToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, TextWriter output)
		{
			using (var sw = new StringWriter()) {
				outputToken.Render(outputProperties, sw, _formatProvider);
				var lines = new StringReader(sw.ToString());
				string nextLine;
				while ((nextLine = lines.ReadLine()) != null) {
					if (nextLine.StartsWith(StackFrameLinePrefix)) {
						StartBaseColors(output, palette);
					} else {
						StartHighlightColors(output, palette);
					}
					output.WriteLine(SecurityElement.Escape(nextLine));
					CloseColors(output);
				}
			}
		}

		private void RenderMessageToken(LogEvent logEvent, Palette palette, TextWriter output)
		{
			foreach (var messageToken in logEvent.MessageTemplate.Tokens) {
				var messagePropertyToken = messageToken as PropertyToken;
				if (messagePropertyToken != null) {
					StartHighlightColors(output, palette);
				} else {
					StartBaseColors(output, palette);
				}
				using (var writer = new StringWriter()) {
					messageToken.Render(logEvent.Properties, writer, _formatProvider);
					output.Write(SecurityElement.Escape(writer.ToString()));
				}
				CloseColors(output);
			}
		}

		private void RenderOutputToken(Palette palette, MessageTemplateToken outputToken, IReadOnlyDictionary<string, LogEventPropertyValue> outputProperties, TextWriter output)
		{
			StartBaseColors(output, palette);
			using (var writer = new StringWriter()) {
				outputToken.Render(outputProperties, writer, _formatProvider);
				output.Write(SecurityElement.Escape(writer.ToString()));
			}
			CloseColors(output);
		}

		private static Palette GetPalette(LogEventLevel level)
		{
			Palette palette;
			if (!LevelPalettes.TryGetValue(level, out palette)) {
				palette = DefaultPalette;
			}
			return palette;
		}

		private static void StartBaseColors(TextWriter output, Palette palette)
		{
			StartColors(output, palette.Base, palette.BaseText);
		}

		private static void StartHighlightColors(TextWriter output, Palette palette)
		{
			StartColors(output, palette.Highlight, palette.HighlightText);
		}

		private static void CloseColors(TextWriter output)
		{
			output.Write("</font>");
		}

		private static void StartColors(TextWriter output, ConsoleColor background, ConsoleColor foreground)
		{
			output.Write("<font style='color:" + foreground + "; background-color:" + background + "'>");
		}


		private readonly IFormatProvider _formatProvider;

		private static readonly Palette DefaultPalette = new Palette {
			Base = ConsoleColor.Black,
			BaseText = ConsoleColor.Gray,
			Highlight = ConsoleColor.DarkGray,
			HighlightText = ConsoleColor.Gray
		};

		private static readonly IDictionary<LogEventLevel, Palette> LevelPalettes = new Dictionary<LogEventLevel, Palette>
        {
            { LogEventLevel.Verbose,     new Palette { Base = ConsoleColor.White, BaseText = ConsoleColor.DarkGray,
                                                       Highlight = ConsoleColor.White, HighlightText = ConsoleColor.Gray } },
            { LogEventLevel.Debug,       new Palette { Base = ConsoleColor.White, BaseText = ConsoleColor.Gray,
                                                       Highlight = ConsoleColor.White, HighlightText = ConsoleColor.Black } },
            { LogEventLevel.Information, new Palette { Base = ConsoleColor.White, BaseText = ConsoleColor.Black,
                                                       Highlight = ConsoleColor.DarkBlue, HighlightText = ConsoleColor.White } },
            { LogEventLevel.Warning,     new Palette { Base = ConsoleColor.White, BaseText = ConsoleColor.Yellow,
                                                       Highlight = ConsoleColor.DarkYellow, HighlightText = ConsoleColor.Black } },
            { LogEventLevel.Error,       new Palette { Base = ConsoleColor.White, BaseText = ConsoleColor.Red,
                                                       Highlight = ConsoleColor.Red, HighlightText = ConsoleColor.White } },
            { LogEventLevel.Fatal,       new Palette { Base = ConsoleColor.DarkRed, BaseText = ConsoleColor.Black,
                                                       Highlight = ConsoleColor.Red, HighlightText = ConsoleColor.White } }
        };

		private readonly object _syncRoot = new object();
		private readonly MessageTemplate _outputTemplate;

		private const string StackFrameLinePrefix = "   ";

		private class Palette
		{
			public ConsoleColor Base { get; set; }
			public ConsoleColor BaseText { get; set; }
			public ConsoleColor Highlight { get; set; }
			public ConsoleColor HighlightText { get; set; }
		}

	}

}
