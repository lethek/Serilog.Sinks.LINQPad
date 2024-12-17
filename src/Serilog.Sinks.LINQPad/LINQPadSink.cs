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

using Serilog.Sinks.LINQPad.Internals;

namespace Serilog.Sinks.LINQPad;

internal class LINQPadSink(ConsoleTheme theme, ITextFormatter formatter, object dumpContainer = null)
    : ILogEventSink, IDisposable
{
    public void Emit(LogEvent logEvent)
    {
        lock (SyncRoot) {
            formatter.Format(logEvent, _writer);
            _api.Dump($"<span style='white-space:pre-wrap'>{_writer}</span>");
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

    private readonly LINQPadApi _api = new(dumpContainer);
    private readonly ThemedHtmlWriter _writer = new(theme ?? throw new ArgumentNullException(nameof(theme)));

    private static readonly object SyncRoot = new();
}