using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Serilog.Sinks.LINQPad.Internals;

internal class LINQPadApi
{
    public static bool IsDarkThemeEnabled => LazyIsDarkThemeEnabled.Value;

    public LINQPadApi(object dumpContainer)
    {
        var rawHtmlMethod = UtilType.GetMethod("RawHtml", [typeof(string)]);
        var rawHtmlFunc = (Func<string, object>)rawHtmlMethod!.CreateDelegate(typeof(Func<string, object>));

#if NETCOREAPP3_1_OR_GREATER
        if (dumpContainer != null) {
            if (!dumpContainer.GetType().IsAssignableFrom(DumpContainerType)) {
                throw new ArgumentException($"The specified dumpContainer must be of type {DumpContainerType.FullName}",
                    nameof(dumpContainer));
            }

            var appendContentMethod = DumpContainerType
                !.GetMethod("AppendContent", 1, [Type.MakeGenericMethodParameter(0), typeof(bool)])
                !.MakeGenericMethod(typeof(object));
            var appendContentFunc =
                (Func<object, bool, object>)appendContentMethod.CreateDelegate(typeof(Func<object, bool, object>),
                    dumpContainer);
            _dumpContent = o => appendContentFunc(rawHtmlFunc(o), false);
        } else {
            var dumpMethod = ExtensionsType
                !.GetMethod("Dump", 1, [Type.MakeGenericMethodParameter(0)])
                !.MakeGenericMethod(typeof(object));
            var dumpFunc = (Func<object, object>)dumpMethod.CreateDelegate(typeof(Func<object, object>));
            _dumpContent = o => dumpFunc(rawHtmlFunc(o));
        }

#elif NET48_OR_GREATER
        if (dumpContainer != null) {
            if (!dumpContainer.GetType().IsAssignableFrom(DumpContainerType)) {
                throw new ArgumentException($"The specified dumpContainer must be of type {DumpContainerType.FullName}",
                    nameof(dumpContainer));
            }

            var verticalRunMethod = UtilType
                !.GetMethod("VerticalRun", [typeof(IEnumerable)]);
            var verticalRunFunc =
                (Func<IEnumerable, object>)verticalRunMethod!.CreateDelegate(typeof(Func<IEnumerable, object>));

            var contentProperty = DumpContainerType!.GetProperty("Content");
            _dumpContent = o => {
                _content.Add(rawHtmlFunc(o));
                contentProperty!.SetValue(dumpContainer, verticalRunFunc(_content), null);
            };
        } else {
            var dumpMethod = ExtensionsType
                !.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.Name == "Dump")
                .Single(x => x.GetParameters().Length == 1)
                .MakeGenericMethod(typeof(object));
            var dumpFunc = (Func<object, object>)dumpMethod.CreateDelegate(typeof(Func<object, object>));
            _dumpContent = o => dumpFunc(rawHtmlFunc(o));
        }

#endif
    }

    
    public void Dump(string content)
        => _dumpContent(content);

    
    private readonly Action<string> _dumpContent;

#if NET48_OR_GREATER
    private readonly List<object> _content = [];
#endif


    private static Type UtilType => LazyUtilType.Value;
    private static Type ExtensionsType => LazyExtensionsType.Value;
    private static Type DumpContainerType => LazyDumpContainerType.Value;


    private static readonly Lazy<Type> LazyUtilType =
        new(() => Type.GetType($"LINQPad.Util, {LINQPadAssemblyName}", true));

    private static readonly Lazy<Type> LazyExtensionsType =
        new(() => Type.GetType($"LINQPad.Extensions, {LINQPadAssemblyName}", true));

    private static readonly Lazy<Type> LazyDumpContainerType =
        new(() => Type.GetType($"LINQPad.DumpContainer, {LINQPadAssemblyName}", true));

    private static readonly Lazy<bool> LazyIsDarkThemeEnabled =
        new(() => (bool)UtilType
            .GetProperty("IsDarkThemeEnabled", BindingFlags.Static | BindingFlags.Public)
            !.GetValue(null, null)!
        );


#if NETCOREAPP3_1_OR_GREATER
    private const string LINQPadAssemblyName = "LINQPad.Runtime";
#elif NET48_OR_GREATER
    private const string LINQPadAssemblyName = "LINQPad";
#endif
}