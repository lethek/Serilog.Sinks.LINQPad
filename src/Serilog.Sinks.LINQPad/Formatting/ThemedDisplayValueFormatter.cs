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

using System;
using System.IO;

using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.LINQPad.Themes;

namespace Serilog.Sinks.LINQPad.Formatting;

internal class ThemedDisplayValueFormatter(ConsoleTheme theme, IFormatProvider formatProvider)
    : ThemedValueFormatter(theme)
{
    public override ThemedValueFormatter SwitchTheme(ConsoleTheme theme)
        => new ThemedDisplayValueFormatter(theme, formatProvider);


    protected override int VisitScalarValue(ThemedValueFormatterState state, ScalarValue scalar)
    {
        if (scalar == null) {
            throw new ArgumentNullException(nameof(scalar));
        }
        return FormatLiteralValue(scalar, state.Output, state.Format);
    }


    protected override int VisitSequenceValue(ThemedValueFormatterState state, SequenceValue sequence)
    {
        if (sequence == null) {
            throw new ArgumentNullException(nameof(sequence));
        }

        var count = 0;

        using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
            state.Output.Write('[');
        }

        var delim = String.Empty;
        foreach (var element in sequence.Elements)
        {
            if (delim.Length != 0) {
                using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
                    state.Output.Write(delim);
                }
            }

            delim = ", ";
            Visit(state, element);
        }

        using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
            state.Output.Write(']');
        }

        return count;
    }


    protected override int VisitStructureValue(ThemedValueFormatterState state, StructureValue structure)
    {
        var count = 0;

        if (structure.TypeTag != null) {
            using (ApplyStyle(state.Output, ConsoleThemeStyle.Name, ref count)) {
                state.Output.Write(structure.TypeTag);
            }

            state.Output.Write(' ');
        }

        using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
            state.Output.Write('{');
        }

        var delim = String.Empty;
        foreach (var property in structure.Properties)
        {
            if (delim.Length != 0) {
                using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
                    state.Output.Write(delim);
                }
            }

            delim = ", ";

            using (ApplyStyle(state.Output, ConsoleThemeStyle.Name, ref count)) {
                state.Output.Write(property.Name);
            }

            using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
                state.Output.Write('=');
            }

            count += Visit(state.Nest(), property.Value);
        }

        using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
            state.Output.Write('}');
        }

        return count;
    }


    protected override int VisitDictionaryValue(ThemedValueFormatterState state, DictionaryValue dictionary)
    {
        var count = 0;

        using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
            state.Output.Write('{');
        }

        var delim = String.Empty;
        foreach (var element in dictionary.Elements) {
            if (delim.Length != 0) {
                using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
                    state.Output.Write(delim);
                }
            }

            delim = ", ";

            using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
                state.Output.Write('[');
            }

            using (ApplyStyle(state.Output, ConsoleThemeStyle.String, ref count)) {
                count += Visit(state.Nest(), element.Key);
            }

            using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
                state.Output.Write("]=");
            }

            count += Visit(state.Nest(), element.Value);
        }

        using (ApplyStyle(state.Output, ConsoleThemeStyle.TertiaryText, ref count)) {
            state.Output.Write('}');
        }

        return count;
    }


    public int FormatLiteralValue(ScalarValue scalar, TextWriter output, string format)
    {
        var value = scalar.Value;
        var count = 0;

        if (value == null) {
            using (ApplyStyle(output, ConsoleThemeStyle.Null, ref count)) {
                output.Write("null");
            }
            return count;
        }

        if (value is string str) {
            using (ApplyStyle(output, ConsoleThemeStyle.String, ref count)) {
                if (format != "l") {
                    JsonValueFormatter.WriteQuotedJsonString(str, output);
                } else {
                    output.Write(str);
                }
            }
            return count;
        }

        if (value is ValueType) {
            if (value is int || value is uint || value is long || value is ulong ||
                value is decimal || value is byte || value is sbyte || value is short ||
                value is ushort || value is float || value is double) {
                using (ApplyStyle(output, ConsoleThemeStyle.Number, ref count)) {
                    scalar.Render(output, format, formatProvider);
                }
                return count;
            }

            if (value is bool b) {
                using (ApplyStyle(output, ConsoleThemeStyle.Boolean, ref count)) {
                    output.Write(b);
                }

                return count;
            }

            if (value is char ch) {
                using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref count)) {
                    output.Write('\'');
                    output.Write(ch);
                    output.Write('\'');
                }
                return count;
            }
        }

        using (ApplyStyle(output, ConsoleThemeStyle.Scalar, ref count)) {
            scalar.Render(output, format, formatProvider);
        }

        return count;
    }
}