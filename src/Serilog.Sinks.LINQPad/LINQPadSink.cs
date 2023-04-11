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
using Serilog.Sinks.LINQPad.Output;
using System.Collections.Generic;

namespace Serilog.Sinks.LINQPad
{

    internal class LINQPadSink : ILogEventSink, IDisposable
    {
        public LINQPadSink(ConsoleTheme theme, ITextFormatter formatter, DumpContainer dumpContainer = null)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _formatter = formatter;
            _writer = new ThemedHtmlWriter(_theme);
            _dumpContainer = dumpContainer;
        }


        public void Emit(LogEvent logEvent)
        {
            lock (SyncRoot) {
                _formatter.Format(logEvent, _writer);

                var rawHtml = Util.RawHtml($"<span style='white-space:pre-wrap'>{_writer}</span>");

                if (_dumpContainer != null) {

#if NETCOREAPP3_1_OR_GREATER
                    _dumpContainer.AppendContent(rawHtml);
#elif NET48_OR_GREATER
                    _content.Add(rawHtml);
                    _dumpContainer.Content = Util.VerticalRun(_content);
#endif
                } else {
                    rawHtml.Dump();
                }
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
        private readonly DumpContainer _dumpContainer;

#if NET48_OR_GREATER
        private readonly List<object> _content = new List<object>();
#endif

        private static readonly object SyncRoot = new object();
    }

}
