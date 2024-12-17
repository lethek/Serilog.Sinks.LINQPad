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
using Serilog.Sinks.LINQPad.Themes;

namespace Serilog.Sinks.LINQPad.Output;

internal class TimestampTokenRenderer(ConsoleTheme theme, PropertyToken token, IFormatProvider formatProvider)
    : OutputTemplateTokenRenderer
{
    public override void Render(LogEvent logEvent, TextWriter output)
    {
        // We need access to ScalarValue.Render() to avoid this alloc; just ensures
        // that custom format providers are supported properly.
        var sv = new ScalarValue(logEvent.Timestamp);

        var _ = 0;
        using (theme.Apply(output, ConsoleThemeStyle.SecondaryText, ref _)) {
            if (token.Alignment == null) {
                sv.Render(output, token.Format, formatProvider);
            } else {
                var buffer = new StringWriter();
                sv.Render(buffer, token.Format, formatProvider);
                var str = buffer.ToString();
                Padding.Apply(output, str, token.Alignment);
            }
        }
    }
}