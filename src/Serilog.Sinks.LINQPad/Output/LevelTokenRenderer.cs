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

using System.Collections.Generic;
using System.IO;

using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.LINQPad.Rendering;
using Serilog.Sinks.LINQPad.Themes;

namespace Serilog.Sinks.LINQPad.Output;

internal class LevelTokenRenderer(ConsoleTheme theme, PropertyToken levelToken) : OutputTemplateTokenRenderer
{
    public override void Render(LogEvent logEvent, TextWriter output)
    {
        var moniker = LevelOutputFormat.GetLevelMoniker(logEvent.Level, levelToken.Format);
        if (!Levels.TryGetValue(logEvent.Level, out var levelStyle)) {
            levelStyle = ConsoleThemeStyle.Invalid;
        }

        var _ = 0;
        using (theme.Apply(output, levelStyle, ref _)) {
            Padding.Apply(output, moniker, levelToken.Alignment);
        }
    }
    
    
    private static readonly Dictionary<LogEventLevel, ConsoleThemeStyle> Levels = new() {
        { LogEventLevel.Verbose, ConsoleThemeStyle.LevelVerbose },
        { LogEventLevel.Debug, ConsoleThemeStyle.LevelDebug },
        { LogEventLevel.Information, ConsoleThemeStyle.LevelInformation },
        { LogEventLevel.Warning, ConsoleThemeStyle.LevelWarning },
        { LogEventLevel.Error, ConsoleThemeStyle.LevelError },
        { LogEventLevel.Fatal, ConsoleThemeStyle.LevelFatal },
    };
}