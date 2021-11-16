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

using LINQPad;

namespace Serilog.Sinks.LINQPad
{

    internal class LINQPadSink : ILogEventSink, IDisposable
    {
        public LINQPadSink(ConsoleTheme theme, ITextFormatter formatter)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _formatter = formatter;
            _writer = new ThemedHtmlWriter(_theme);
        }


        public void Emit(LogEvent logEvent)
        {
            lock (SyncRoot) {
                _formatter.Format(logEvent, _writer);
                Util.RawHtml($"<span style='white-space:pre-wrap'>{_writer}</span>").Dump();
                _writer.Clear();
            }
        }


        public void Dispose()
        {
            if (!_disposed) {
                _writer.Dispose();
                _disposed = true;
            }
        }


        private bool _disposed;


        private readonly ConsoleTheme _theme;
        private readonly ITextFormatter _formatter;
        private readonly ThemedHtmlWriter _writer;

        private static readonly object SyncRoot = new object();
    }

}
