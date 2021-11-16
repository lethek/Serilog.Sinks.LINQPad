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

using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.LINQPad.Rendering;
using Serilog.Sinks.LINQPad.Formatting;
using Serilog.Sinks.LINQPad.Themes;

namespace Serilog.Sinks.LINQPad.Output
{
    internal class MessageTemplateOutputTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly ConsoleTheme _theme;
        private readonly PropertyToken _token;
        private readonly ThemedMessageTemplateRenderer _renderer;

        public MessageTemplateOutputTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider formatProvider)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _token = token ?? throw new ArgumentNullException(nameof(token));
            bool isLiteral = false, isJson = false;

            if (token.Format != null) {
                for (var i = 0; i < token.Format.Length; ++i) {
                    if (token.Format[i] == 'l') {
                        isLiteral = true;
                    } else if (token.Format[i] == 'j') {
                        isJson = true;
                    }
                }
            }

            var valueFormatter = isJson
                ? (ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)
                : new ThemedDisplayValueFormatter(theme, formatProvider);

            _renderer = new ThemedMessageTemplateRenderer(theme, valueFormatter, isLiteral);
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            if (_token.Alignment == null || !_theme.CanBuffer) {
                _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, output);
                return;
            }

            var buffer = new StringWriter();
            var invisible = _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, buffer);
            var value = buffer.ToString();
            Padding.Apply(output, value, _token.Alignment.Value.Widen(invisible));
        }
    }
}