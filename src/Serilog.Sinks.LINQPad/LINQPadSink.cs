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

using System.Collections.Generic;
using System.Linq;

namespace Serilog.Sinks.LINQPad;

internal class LINQPadSink : ILogEventSink, IDisposable
{
    public LINQPadSink(ConsoleTheme theme, ITextFormatter formatter, object dumpContainer = null)
    {
        _formatter = formatter;
        _writer = new ThemedHtmlWriter(theme ?? throw new ArgumentNullException(nameof(theme)));
        _dumpContainer = dumpContainer;
        ReflectLINQPadRuntime();
    }


    public void Emit(LogEvent logEvent)
    {
        lock (SyncRoot) {
            _formatter.Format(logEvent, _writer);
            _dumpContent($"<span style='white-space:pre-wrap'>{_writer}</span>");
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


    private void ReflectLINQPadRuntime()
    {
#if NETCOREAPP3_1_OR_GREATER
        var utilType = Type.GetType("LINQPad.Util, LINQPad.Runtime");
        var rawHtmlMethod = utilType!.GetMethod("RawHtml", new[] { typeof(string) });
        var rawHtmlFunc = (Func<string, object>)rawHtmlMethod!.CreateDelegate(typeof(Func<string, object>));

        if (_dumpContainer != null) {
            var dumpContainerType = Type.GetType("LINQPad.DumpContainer, LINQPad.Runtime");
            if (!_dumpContainer.GetType().IsAssignableFrom(dumpContainerType)) {
                throw new ArgumentException($"The specified dumpContainer must be of type {dumpContainerType.FullName}", nameof(_dumpContainer));
            }
            var appendContentMethod = dumpContainerType
                !.GetMethod("AppendContent", 1, new[] { Type.MakeGenericMethodParameter(0), typeof(bool) })
                !.MakeGenericMethod(typeof(object));
            var appendContentFunc = (Func<object, bool, object>)appendContentMethod.CreateDelegate(typeof(Func<object, bool, object>), _dumpContainer);
            _dumpContent = o => appendContentFunc(rawHtmlFunc(o), false);

        } else {
            var extensionsType = Type.GetType("LINQPad.Extensions, LINQPad.Runtime");
            var dumpMethod = extensionsType
                !.GetMethod("Dump", 1, new[] { Type.MakeGenericMethodParameter(0) })
                !.MakeGenericMethod(typeof(object));
            var dumpFunc = (Func<object, object>)dumpMethod.CreateDelegate(typeof(Func<object, object>));
            _dumpContent = o => dumpFunc(rawHtmlFunc(o));
        }

#elif NET48_OR_GREATER
            var utilType = Type.GetType("LINQPad.Util, LINQPad");
            var rawHtmlMethod = utilType!.GetMethod("RawHtml", new[] { typeof(string) });
            var rawHtmlFunc = (Func<string, object>)rawHtmlMethod!.CreateDelegate(typeof(Func<string, object>));

            if (_dumpContainer != null) {
                var dumpContainerType = Type.GetType("LINQPad.DumpContainer, LINQPad");
                if (!_dumpContainer.GetType().IsAssignableFrom(dumpContainerType)) {
                    throw new ArgumentException($"The specified dumpContainer must be of type {dumpContainerType.FullName}", nameof(_dumpContainer));
                }

                var verticalRunMethod = utilType
                    !.GetMethod("VerticalRun", new[] { typeof(object[]) });
                var verticalRunFunc = (Func<object[], object>)verticalRunMethod!.CreateDelegate(typeof(Func<object[], object>));

                var contentProperty = dumpContainerType!.GetProperty("Content");
                _dumpContent = o => {
                    _content.Add(o);
                    contentProperty!.SetValue(_dumpContainer, verticalRunFunc(new[] { rawHtmlFunc(o) }), null);
                };

            } else {
                var extensionsType = Type.GetType("LINQPad.Extensions, LINQPad");
                var dumpMethod = extensionsType
                    !.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .Where(x => x.Name == "Dump")
                    .Single(x => x.GetParameters().Length == 1)                
                    .MakeGenericMethod(typeof(object));
                var dumpFunc = (Func<object, object>)dumpMethod.CreateDelegate(typeof(Func<object, object>));
                _dumpContent = o => dumpFunc(rawHtmlFunc(o));
            }
#endif
    }


    private bool _disposed;
    private Action<string> _dumpContent;

    private readonly ITextFormatter _formatter;
    private readonly ThemedHtmlWriter _writer;
    private readonly object _dumpContainer;

#if NET48_OR_GREATER
        private readonly List<object> _content = new();
#endif

    private static readonly object SyncRoot = new();
}