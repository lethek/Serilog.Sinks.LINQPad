// Copyright 2017 Serilog Contributors
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

using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.LINQPad;
using Serilog.Sinks.LINQPad.Output;
using Serilog.Sinks.LINQPad.Themes;
using System;

using LINQPad;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.Console() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class ConsoleLoggerConfigurationExtensions
    {
        private const string DefaultConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to <see cref="System.Console"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// The default is <code>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</code>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="theme">The theme to apply to the styled output. If not specified,
        /// uses <see cref="LINQPadTheme.LINQPadLiterate"/> or if dark-mode is enabled <see cref="LINQPadTheme.LINQPadDark"/>.</param>
        /// <param name="dumpContainer">Optional write to a specified <see cref="DumpContainer"/> and not direct to the result panel.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration LINQPad(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultConsoleOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null,
            ConsoleTheme theme = null,
            DumpContainer dumpContainer = null
            )
        {
            if (sinkConfiguration == null) {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (outputTemplate == null) {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var appliedTheme = theme ?? (Util.IsDarkThemeEnabled ? DefaultThemes.LINQPadDark : DefaultThemes.LINQPadLiterate);

            var formatter = new OutputTemplateRenderer(appliedTheme, outputTemplate, formatProvider);
            return sinkConfiguration.Sink(new LINQPadSink(appliedTheme, formatter, dumpContainer), restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Writes log events to <see cref="System.Console"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="formatter">Controls the rendering of log events into text, for example to log JSON. To
        /// control plain text formatting, use the overload that accepts an output template.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <param name="dumpContainer">Optional write to a specified <see cref="DumpContainer"/> and not direct to the result panel.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration LINQPad(
            this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null,
            DumpContainer dumpContainer = null)
        {
            if (sinkConfiguration == null) {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (formatter == null) {
                throw new ArgumentNullException(nameof(formatter));
            }

            return sinkConfiguration.Sink(new LINQPadSink(DefaultThemes.None, formatter, dumpContainer), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
