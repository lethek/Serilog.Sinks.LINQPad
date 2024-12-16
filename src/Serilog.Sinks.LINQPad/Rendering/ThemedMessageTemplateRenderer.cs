﻿// Copyright 2017 Serilog Contributors
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

using System;
using System.Collections.Generic;
using System.IO;

using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.LINQPad.Formatting;
using Serilog.Sinks.LINQPad.Themes;

namespace Serilog.Sinks.LINQPad.Rendering;

internal class ThemedMessageTemplateRenderer
{
    public ThemedMessageTemplateRenderer(ConsoleTheme theme, ThemedValueFormatter valueFormatter, bool isLiteral)
    {
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        _valueFormatter = valueFormatter;
        _isLiteral = isLiteral;
        _unthemedValueFormatter = valueFormatter.SwitchTheme(NoTheme);
    }


    public int Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
    {
        var count = 0;
        foreach (var token in template.Tokens) {
            if (token is TextToken tt) {
                count += RenderTextToken(tt, output);
            } else {
                var pt = (PropertyToken)token;
                count += RenderPropertyToken(pt, properties, output);
            }
        }
        return count;
    }


    private int RenderTextToken(TextToken tt, TextWriter output)
    {
        var count = 0;
        using (_theme.Apply(output, ConsoleThemeStyle.Text, ref count)) {
            output.Write(tt.Text);
        }

        return count;
    }


    private int RenderPropertyToken(PropertyToken pt, IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
    {
        if (!properties.TryGetValue(pt.PropertyName, out var propertyValue)) {
            var count = 0;
            using (_theme.Apply(output, ConsoleThemeStyle.Invalid, ref count)) {
                output.Write(pt.ToString());
            }

            return count;
        }

        if (!pt.Alignment.HasValue) {
            return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
        }

        var valueOutput = new StringWriter();

        if (!_theme.CanBuffer) {
            return RenderAlignedPropertyTokenUnbuffered(pt, output, propertyValue);
        }

        var invisibleCount = RenderValue(_theme, _valueFormatter, propertyValue, valueOutput, pt.Format);

        var value = valueOutput.ToString();

        if (value.Length - invisibleCount >= pt.Alignment.Value.Width) {
            output.Write(value);
        } else {
            Padding.Apply(output, value, pt.Alignment.Value.Widen(invisibleCount));
        }

        return invisibleCount;
    }


    private int RenderAlignedPropertyTokenUnbuffered(PropertyToken pt, TextWriter output, LogEventPropertyValue propertyValue)
    {
        var valueOutput = new StringWriter();
        RenderValue(NoTheme, _unthemedValueFormatter, propertyValue, valueOutput, pt.Format);

        var valueLength = valueOutput.ToString().Length;
        // ReSharper disable once PossibleInvalidOperationException
        if (valueLength >= pt.Alignment.Value.Width) {
            return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
        }

        if (pt.Alignment.Value.Direction == AlignmentDirection.Left) {
            var invisible = RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
            Padding.Apply(output, String.Empty, pt.Alignment.Value.Widen(-valueLength));
            return invisible;
        }

        Padding.Apply(output, String.Empty, pt.Alignment.Value.Widen(-valueLength));
        return RenderValue(_theme, _valueFormatter, propertyValue, output, pt.Format);
    }


    private int RenderValue(ConsoleTheme theme, ThemedValueFormatter valueFormatter, LogEventPropertyValue propertyValue, TextWriter output, string format)
    {
        if (_isLiteral && propertyValue is ScalarValue sv && sv.Value is string) {
            var count = 0;
            using (theme.Apply(output, ConsoleThemeStyle.String, ref count)) {
                output.Write(sv.Value);
            }

            return count;
        }

        return valueFormatter.Format(propertyValue, output, format, _isLiteral);
    }


    private readonly bool _isLiteral;
    private readonly ConsoleTheme _theme;
    private readonly ThemedValueFormatter _valueFormatter;
    private readonly ThemedValueFormatter _unthemedValueFormatter;


    private static readonly ConsoleTheme NoTheme = new EmptyConsoleTheme();
}