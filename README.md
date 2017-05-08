Serilog.Sinks.LINQPad
=====================
Version 2.0
-----------

Serilog sink to publish colored output to the LINQPad Results panel. At present, a large percentage of the code has been shamelessly ripped from [LiterateConsoleSink](https://github.com/serilog/serilog-sinks-literate), with modifications to allow LINQPad to display colored results and the ability to customize the color scheme.

**Important:** This NuGet package is only useful (and usable) when using Serilog within LINQPad scripts.

### Getting started
Install the [Serilog.Sinks.LINQPad](https://nuget.org/packages/serilog.sinks.linqpad) package from NuGet:
```
PM> Install-Package Serilog.Sinks.LINQPad
```

To configure the sink in C# code, call `WriteTo.LINQPad()` during logger configuration:
```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.LINQPad()
    .CreateLogger();
```

Log events will be printed with color to the LINQPad results-panel.

### Custom color schemes
By default, the LINQPad sink will use colors which look better on a white/light background than a darker one. However, if you have customized your "Style sheet for text (HTML) results" in the LINQPad Preferences window, you might find that the colours used by the sink are no longer readable. Or they might simply just not be to your taste.

If that's the case, see these examples for how to customize the colors:

```csharp
using Serilog.Sinks.LINQPad;

//Create a Serilog.Sinks.LINQPad.ColorScheme instance. It may be created with default via a factory method:
colorScheme = ColorScheme.CreateLightScheme(); //Similar to the LiterateConsoleSink's coloring, but modified for white backgrounds
colorScheme = ColorScheme.CreateDarkScheme(); //Almost identical to the LiterateConsoleSink's coloring, better for black backgrounds

//Or you may directly create the object (this is equivalent to calling ColorScheme.CreateLightScheme()):
colorScheme = new ColorScheme();

//The colorScheme's properties may be modified at any time, even after the Logger has been configured, in order to affect subsequent log-events.
colorScheme.Text = new ColorPair(Color.Aqua, Color.Blue); //Text will use Aqua foreground on a Blue background
colorScheme.Text = new ColorPair(Color.Aqua); //Text will use Aqua foreground on the panel's default background
colorScheme.Text = new ColorPair(background: Color.Blue); //Text will use the panel's default foreground color on a Blue background

colorScheme.ErrorLevel.Foreground = Color.Crimson; // The Error level token will render with Crimson foreground text
colorScheme.ErrorLevel.Background = null; //The Error level token will render with the panel's default background color

//Pass an instance of ColorScheme to the sink during logger configuration.
Log.Logger = new LoggerConfiguration()
    .WriteTo.LINQPad(colorScheme: colorScheme)
    .CreateLogger();
```

Alternatively, if no ColorScheme is specified when the sink is configured, it will automatically use the static `LINQPadSink.DefaultColors` property which is created internally from the `ColorScheme.CreateLightScheme()` factory method. It can also be modified but could affect other instances of the sink:
```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.LINQPad()
    .CreateLogger();

LINQPadSink.DefaultColors.StringSymbol = new ColorPair(Color.DeepSkyBlue);
```

The complete list of ColorScheme properties is:
`Text`, `Subtext`, `Punctuation`, `VerboseLevel`, `DebugLevel`, `InformationLevel`, `WarningLevel`, `ErrorLevel`, `FatalLevel`, `KeywordSymbol`, `NumericSymbol`, `StringSymbol`, `OtherSymbol`, `NameSymbol`, `RawText`.


### Output templates

The format of events to the console can be modified using the `outputTemplate` configuration parameter:

```csharp
    .WriteTo.LINQPad(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}{Exception}"
    )
```

The default template, shown in the example above, uses built-in properties like `Timestamp` and `Level`. Properties from events, including those attached using [enrichers](https://github.com/serilog/serilog/wiki/Enrichment), can also appear in the output template.

For more compact level names, use a format such as `{Level:u3}` or `{Level:w3}` for three-character upper- or lowercase level names, respectively.

_Copyright &copy; 2016 Serilog Contributors - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
