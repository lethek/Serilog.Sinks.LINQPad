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

using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.LINQPad.Themes;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using LINQPad;

namespace Serilog.Sinks.LINQPad
{
    class LINQPadSink : ILogEventSink
    {
        readonly LogEventLevel? _standardErrorFromLevel;
        readonly ConsoleTheme _theme;
        readonly ITextFormatter _formatter;
        static readonly object _syncRoot = new object();

        const int DefaultWriteBufferCapacity = 256;

        public LINQPadSink(
            ConsoleTheme theme,
            ITextFormatter formatter,
            LogEventLevel? standardErrorFromLevel)
        {
            _standardErrorFromLevel = standardErrorFromLevel;
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            using (var buffer = new ThemedHtmlWriter(_theme, new StringBuilder(DefaultWriteBufferCapacity))) {
                _theme.Reset(buffer);
                _formatter.Format(logEvent, buffer);
                //_theme.Dump();
                lock (_syncRoot) {
                    Util.RawHtml($"<span style='white-space:pre-wrap'>{buffer}</span>").Dump($"<span style='white-space:pre-wrap'>{buffer}</span>");
                }
            }
        }
    }
}
