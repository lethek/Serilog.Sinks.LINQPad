﻿using System.Globalization;

using Serilog.Events;
using Serilog.Sinks.LINQPad.Support;
using Serilog.Sinks.LINQPad.Themes;

using Xunit;


namespace Serilog.Sinks.LINQPad.Output;

public class OutputTemplateRendererTests
{
    [Fact]
    public void UsesFormatProvider()
    {
        var french = new CultureInfo("fr-FR");
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message}", french);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("{0}", 12.345));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("12,345", sw.ToString());
    }


    [Fact]
    public void MessageTemplatesContainingFormatStringPropertiesRenderCorrectly()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("{Message}", "Hello, world!"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("\"Hello, world!\"", sw.ToString());
    }


    [Fact]
    public void UppercaseFormatSpecifierIsSupportedForStrings()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Name:u}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("{Name}", "Nick"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("NICK", sw.ToString());
    }


    [Fact]
    public void LowercaseFormatSpecifierIsSupportedForStrings()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Name:w}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("{Name}", "Nick"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("nick", sw.ToString());
    }


    [Theory]
    [InlineData(LogEventLevel.Verbose, 1, "V")]
    [InlineData(LogEventLevel.Verbose, 2, "Vb")]
    [InlineData(LogEventLevel.Verbose, 3, "Vrb")]
    [InlineData(LogEventLevel.Verbose, 4, "Verb")]
    [InlineData(LogEventLevel.Verbose, 5, "Verbo")]
    [InlineData(LogEventLevel.Verbose, 6, "Verbos")]
    [InlineData(LogEventLevel.Verbose, 7, "Verbose")]
    [InlineData(LogEventLevel.Verbose, 8, "Verbose")]
    [InlineData(LogEventLevel.Debug, 1, "D")]
    [InlineData(LogEventLevel.Debug, 2, "De")]
    [InlineData(LogEventLevel.Debug, 3, "Dbg")]
    [InlineData(LogEventLevel.Debug, 4, "Dbug")]
    [InlineData(LogEventLevel.Debug, 5, "Debug")]
    [InlineData(LogEventLevel.Debug, 6, "Debug")]
    [InlineData(LogEventLevel.Information, 1, "I")]
    [InlineData(LogEventLevel.Information, 2, "In")]
    [InlineData(LogEventLevel.Information, 3, "Inf")]
    [InlineData(LogEventLevel.Information, 4, "Info")]
    [InlineData(LogEventLevel.Information, 5, "Infor")]
    [InlineData(LogEventLevel.Information, 6, "Inform")]
    [InlineData(LogEventLevel.Information, 7, "Informa")]
    [InlineData(LogEventLevel.Information, 8, "Informat")]
    [InlineData(LogEventLevel.Information, 9, "Informati")]
    [InlineData(LogEventLevel.Information, 10, "Informatio")]
    [InlineData(LogEventLevel.Information, 11, "Information")]
    [InlineData(LogEventLevel.Information, 12, "Information")]
    [InlineData(LogEventLevel.Error, 1, "E")]
    [InlineData(LogEventLevel.Error, 2, "Er")]
    [InlineData(LogEventLevel.Error, 3, "Err")]
    [InlineData(LogEventLevel.Error, 4, "Eror")]
    [InlineData(LogEventLevel.Error, 5, "Error")]
    [InlineData(LogEventLevel.Error, 6, "Error")]
    [InlineData(LogEventLevel.Fatal, 1, "F")]
    [InlineData(LogEventLevel.Fatal, 2, "Fa")]
    [InlineData(LogEventLevel.Fatal, 3, "Ftl")]
    [InlineData(LogEventLevel.Fatal, 4, "Fatl")]
    [InlineData(LogEventLevel.Fatal, 5, "Fatal")]
    [InlineData(LogEventLevel.Fatal, 6, "Fatal")]
    [InlineData(LogEventLevel.Warning, 1, "W")]
    [InlineData(LogEventLevel.Warning, 2, "Wn")]
    [InlineData(LogEventLevel.Warning, 3, "Wrn")]
    [InlineData(LogEventLevel.Warning, 4, "Warn")]
    [InlineData(LogEventLevel.Warning, 5, "Warni")]
    [InlineData(LogEventLevel.Warning, 6, "Warnin")]
    [InlineData(LogEventLevel.Warning, 7, "Warning")]
    [InlineData(LogEventLevel.Warning, 8, "Warning")]
    public void FixedLengthLevelIsSupported(LogEventLevel level,
        int width,
        string expected)
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, $"{{Level:t{width}}}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Write(level, "Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal(expected, sw.ToString());
    }


    [Fact]
    public void FixedLengthLevelSupportsUpperCasing()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Level:u3}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("INF", sw.ToString());
    }


    [Fact]
    public void FixedLengthLevelSupportsLowerCasing()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Level:w3}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("inf", sw.ToString());
    }


    [Fact]
    public void DefaultLevelLengthIsFullText()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Level}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("Information", sw.ToString());
    }


    [Fact]
    public void AligmentAndWidthCanBeCombined()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Level,5:w3}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("  inf", sw.ToString());
    }

    private enum Size
    {
        Large
    }

    private class SizeFormatter(IFormatProvider innerFormatProvider) : IFormatProvider, ICustomFormatter
    {
        public object? GetFormat(Type? formatType)
            => formatType == typeof(ICustomFormatter) ? this : innerFormatProvider.GetFormat(formatType);


        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg is Size size) {
                return size == Size.Large ? "Huge" : size.ToString();
            }

            if (arg is IFormattable formattable) {
                return formattable.ToString(format, innerFormatProvider);
            }

            return arg?.ToString() ?? String.Empty;
        }
    }


    [Fact]
    public void AppliesCustomFormatterToEnums()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message}", new SizeFormatter(CultureInfo.InvariantCulture));
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Size {Size}", Size.Large));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("Size Huge", sw.ToString());
    }


    [Fact]
    public void NonMessagePropertiesAreRendered()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Properties}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.ForContext("Foo", 42).Information("Hello from {Bar}!", "bar"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("{Foo=42}", sw.ToString());
    }


    [Fact]
    public void NonMessagePositionalPropertiesAreRendered()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Properties}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.ForContext("Foo", 42).Information("Hello from {0}!", "bar"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("{Foo=42}", sw.ToString());
    }


    [Fact]
    public void DoNotDuplicatePropertiesAlreadyRenderedInOutputTemplate()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Foo} {Properties}", CultureInfo.InvariantCulture);
        var evt = DelegatingSink.GetLogEvent(l => l.ForContext("Foo", 42).ForContext("Bar", 42).Information("Hello from bar!"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal("42 {Bar=42}", sw.ToString());
    }


    [Theory]
    [InlineData("", "Hello, \"World\"!")]
    [InlineData(":j", "Hello, \"World\"!")]
    [InlineData(":l", "Hello, World!")]
    [InlineData(":lj", "Hello, World!")]
    [InlineData(":jl", "Hello, World!")]
    public void AppliesLiteralFormattingToMessageStringsWhenSpecified(string format, string expected)
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message" + format + "}", null);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Hello, {Name}!", "World"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal(expected, sw.ToString());
    }


    [Theory]
    [InlineData("", "{Name=\"World\"}")]
    [InlineData(":j", "{\"Name\": \"World\"}")]
    [InlineData(":lj", "{\"Name\": \"World\"}")]
    [InlineData(":jl", "{\"Name\": \"World\"}")]
    public void AppliesJsonFormattingToMessageStructuresWhenSpecified(string format, string expected)
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message" + format + "}", null);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("{@Obj}", new { Name = "World" }));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal(expected, sw.ToString());
    }


    [Theory]
    [InlineData("", "{Name=\"World\"}")]
    [InlineData(":j", "{\"Name\": \"World\"}")]
    [InlineData(":lj", "{\"Name\": \"World\"}")]
    [InlineData(":jl", "{\"Name\": \"World\"}")]
    public void AppliesJsonFormattingToPropertiesTokenWhenSpecified(string format, string expected)
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Properties" + format + "}", null);
        var evt = DelegatingSink.GetLogEvent(l => l.ForContext("Name", "World").Information("Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);
        Assert.Equal(expected, sw.ToString());
    }


    [Fact]
    public void AnEmptyPropertiesTokenIsAnEmptyStructureValue()
    {
        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Properties}", null);
        var evt = DelegatingSink.GetLogEvent(l => l.Information("Hello"));
        using var sw = new StringWriter();
        formatter.Format(evt, sw);

        // /!\ different behavior from Serilog Core : https://github.com/serilog/serilog/blob/5c3a7821aa0f654e551dc21e8e19089f6767666b/test/Serilog.Tests/Formatting/Display/MessageTemplateTextFormatterTests.cs#L268-L278
        //
        // var expected = new StructureValue(Enumerable.Empty<LogEventProperty>()).ToString();
        // // expected == "{  }"
        // Assert.Equal(expected, sw.ToString());
        //
        Assert.Equal("{}", sw.ToString());
    }


    [Theory]
    [InlineData("", true)]
    [InlineData(":lj", true)]
    [InlineData(":jl", true)]
    [InlineData(":j", false)]
    [InlineData(":l", true)]
    public void FormatProviderWithScalarProperties(string format, bool shouldUseCustomFormatter)
    {
        var frenchFormatProvider = new CultureInfo("fr-FR");
        var defaultFormatProvider = CultureInfo.InvariantCulture;

        var date = new DateTime(2018, 01, 01);
        var number = 12.345;

        var expectedFormattedDate = shouldUseCustomFormatter
            ? date.ToString(frenchFormatProvider)
            : date.ToString("O", defaultFormatProvider);
        var expectedFormattedNumber = shouldUseCustomFormatter
            ? number.ToString(frenchFormatProvider)
            : number.ToString(defaultFormatProvider);

        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message" + format + "}", frenchFormatProvider);
        var evt = DelegatingSink.GetLogEvent(l => { l.Information("{MyDate}{MyNumber}", date, number); });
        using var sw = new StringWriter();
        formatter.Format(evt, sw);

        Assert.Contains(expectedFormattedDate, sw.ToString());
        Assert.Contains(expectedFormattedNumber, sw.ToString());
    }


    [Theory]
    [InlineData("", true)]
    [InlineData(":lj", false)]
    [InlineData(":jl", false)]
    [InlineData(":j", false)]
    [InlineData(":l", true)]
    public void FormatProviderWithDestructuredProperties(string format, bool shouldUseCustomFormatter)
    {
        var frenchFormatProvider = new CultureInfo("fr-FR");
        var defaultFormatProvider = CultureInfo.InvariantCulture;

        var date = new DateTime(2018, 01, 01);
        var number = 12.345;

        var expectedFormattedDate = shouldUseCustomFormatter
            ? date.ToString(frenchFormatProvider)
            : date.ToString("O", defaultFormatProvider);
        var expectedFormattedNumber = shouldUseCustomFormatter
            ? number.ToString(frenchFormatProvider)
            : number.ToString(defaultFormatProvider);

        var formatter = new OutputTemplateRenderer(DefaultThemes.None, "{Message" + format + "}", frenchFormatProvider);
        var evt = DelegatingSink.GetLogEvent(
            l => {
                l.Information(
                    "{@Item}", new {
                        MyDate = date,
                        MyNumber = number,
                    }
                );
            }
        );
        using var sw = new StringWriter();
        formatter.Format(evt, sw);

        Assert.Contains(expectedFormattedDate, sw.ToString());
        Assert.Contains(expectedFormattedNumber, sw.ToString());
    }
}