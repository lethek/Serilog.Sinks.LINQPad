Serilog.Sinks.LINQPad
=====================
Version 3.1
-----------

Serilog sink to publish colored output to the LINQPad Results panel. At present, a large percentage of the code has been shamelessly ripped from [ConsoleSink](https://github.com/serilog/serilog-sinks-console), with modifications to allow LINQPad to display colored results and the ability to customize the color scheme.

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
By default, the LINQPad sink will use colors which look better on a white/light background than a darker one. However, if you have customized your "Style sheet for text (HTML) results" in the LINQPad Preferences window, you might find that the colors used by the sink are no longer readable. Or they might simply just not be to your taste.

If that's the case, see these examples for how to customize the colors:

```csharp
using Serilog.Sinks.LINQPad;
using Serilog.Sinks.LINQPad.Themes;

//Either get a pre-defined Serilog.Sinks.LINQPad.Themes.ConsoleTheme instance, there are several static defaults here:
theme = DefaultThemes.LINQPadLiterate; //This is the default normally. Based on the Literate theme from the ConsoleSink project, but modified for white backgrounds.
theme = DefaultThemes.LINQPadDark; //This is the default for dark-mode.
theme = DefaultThemes.LINQPadColored; //Similar to the Colored theme in the ConsoleSink project. It's been modified to look better on white backgrounds.
theme = DefaultThemes.Literate; //Identical to the Literate theme (which is the default) in the ConsoleSink project. Designed for black console backgrounds.
theme = DefaultThemes.Colored; //Identical to the Colored theme in the ConsoleSink project. Designed for black console backgrounds.
theme = DefaultThemes.Grayscale; //Identical to the Grayscale theme in the ConsoleSink project. Designed for black console backgrounds.

//Or create a new theme from scratch:
theme = new LINQPadTheme(
    new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
        [ConsoleThemeStyle.Text] = new LINQPadThemeStyle { Foreground = Color.Black },
        [ConsoleThemeStyle.SecondaryText] = new LINQPadThemeStyle { Foreground = Color.Gray },
        [ConsoleThemeStyle.TertiaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
        [ConsoleThemeStyle.Invalid] = new LINQPadThemeStyle { Background = Color.Yellow, Italic = true },
        [ConsoleThemeStyle.Null] = new LINQPadThemeStyle { Foreground = Color.Blue },
        [ConsoleThemeStyle.Name] = new LINQPadThemeStyle { Foreground = Color.Gray },
        [ConsoleThemeStyle.String] = new LINQPadThemeStyle { Foreground = Color.DarkCyan, Bold = true },
        [ConsoleThemeStyle.Number] = new LINQPadThemeStyle { Foreground = Color.Magenta, Bold = true },
        [ConsoleThemeStyle.Boolean] = new LINQPadThemeStyle { Foreground = Color.Blue, Bold = true },
        [ConsoleThemeStyle.Scalar] = new LINQPadThemeStyle { Foreground = Color.Green, Bold = true },
        [ConsoleThemeStyle.LevelVerbose] = new LINQPadThemeStyle { Foreground = Color.LightGray },
        [ConsoleThemeStyle.LevelDebug] = new LINQPadThemeStyle { Foreground = Color.Gray },
        [ConsoleThemeStyle.LevelInformation] = new LINQPadThemeStyle { Foreground = Color.Black },
        [ConsoleThemeStyle.LevelWarning] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.Yellow },
        [ConsoleThemeStyle.LevelError] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red },
        [ConsoleThemeStyle.LevelFatal] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red, Bold = true },
    });

//A theme's properties may be modified at any time, even after the Logger has been configured, in order to affect subsequent log-events.
theme.Styles[ConsoleThemeStyle.Text] = new LINQPadThemeStyle(Color.Aqua, Color.Blue, italic:true); //Text will use an italic Aqua foreground on a Blue background
theme.Styles[ConsoleThemeStyle.Text] = new LINQPadThemeStyle(Color.Aqua); //Text will use Aqua foreground on the panel's default background
theme.Styles[ConsoleThemeStyle.Text] = new LINQPadThemeStyle(background: Color.Blue); //Text will use the panel's default foreground color on a Blue background

theme.Styles[ConsoleThemeStyle.LevelError].Foreground = Color.Crimson; // The Error level token will render with Crimson foreground text
theme.Styles[ConsoleThemeStyle.LevelError].Background = null; //The Error level token will render with the panel's default background color

//Pass an instance of the theme to the sink during logger configuration.
Log.Logger = new LoggerConfiguration()
    .WriteTo.LINQPad(theme: theme)
    .CreateLogger();
```

Alternatively, if no theme is specified when the sink is configured, it will automatically choose one of the defaults: either the static `DefaultThemes.LINQPadLiterate` when LINQPad is using its Windows Default theme or `DefaultThemes.LINQPadDark` when LINQPad is using its Dark theme.
```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.LINQPad()
    .CreateLogger();
```

Note: the static default themes can be modified but could affect other instances of the sink:
```
DefaultThemes.LINQPadLiterate[ConsoleThemeStyle.String] = new LINQPadThemeStyle(Color.DeepSkyBlue);
```

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
