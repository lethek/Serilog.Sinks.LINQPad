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
using System.Text;

using LINQPad;


namespace Serilog.Sinks.LINQPad
{

    internal class LINQPadSink : ILogEventSink
    {
        public LINQPadSink(ConsoleTheme theme, ITextFormatter formatter)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _formatter = formatter;
        }


        public void Emit(LogEvent logEvent)
        {
            using (var buffer = new ThemedHtmlWriter(_theme, new StringBuilder(DefaultWriteBufferCapacity))) {
                lock (SyncRoot) {
                    _formatter.Format(logEvent, buffer);
                    Util.RawHtml($"<span style='white-space:pre-wrap'>{buffer}</span>").Dump();
                }
            }
        }


        private readonly ConsoleTheme _theme;
        private readonly ITextFormatter _formatter;


        private const int DefaultWriteBufferCapacity = 256;


        private static readonly object SyncRoot = new object();
    }

}
