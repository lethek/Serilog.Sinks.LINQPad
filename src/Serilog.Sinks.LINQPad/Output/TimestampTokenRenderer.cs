﻿// Copyright 2017 Serilog Contributors
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
using Serilog.Sinks.LINQPad.Themes;

namespace Serilog.Sinks.LINQPad.Output
{
    internal class TimestampTokenRenderer : OutputTemplateTokenRenderer
    {
        public TimestampTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider formatProvider)
        {
            _theme = theme;
            _token = token;
            _formatProvider = formatProvider;
        }


        public override void Render(LogEvent logEvent, TextWriter output)
        {
            // We need access to ScalarValue.Render() to avoid this alloc; just ensures
            // that custom format providers are supported properly.
            var sv = new ScalarValue(logEvent.Timestamp);

            var _ = 0;
            using (_theme.Apply(output, ConsoleThemeStyle.SecondaryText, ref _)) {
                if (_token.Alignment == null) {
                    sv.Render(output, _token.Format, _formatProvider);
                } else {
                    var buffer = new StringWriter();
                    sv.Render(buffer, _token.Format, _formatProvider);
                    var str = buffer.ToString();
                    Padding.Apply(output, str, _token.Alignment);
                }
            }
        }


        private readonly ConsoleTheme _theme;
        private readonly PropertyToken _token;
        private readonly IFormatProvider _formatProvider;
    }
}
