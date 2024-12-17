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
using System.Drawing;


namespace Serilog.Sinks.LINQPad.Themes;

public static class DefaultThemes
{
    /// <summary>
    /// No styling applied.
    /// </summary>
    public static ConsoleTheme None { get; } = new EmptyConsoleTheme();


    public static LINQPadTheme LINQPadDark { get; } = new(
        new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
            [ConsoleThemeStyle.Text] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = Color.LightGray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.Invalid] = new() { Foreground = Color.Red, Background = Color.Yellow, Italic = true },
            [ConsoleThemeStyle.Null] = new() { Foreground = Color.Yellow, Italic = true },
            [ConsoleThemeStyle.Name] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.String] = new() { Foreground = Color.Cyan, Bold = true },
            [ConsoleThemeStyle.Number] = new() { Foreground = Color.Magenta, Bold = true },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = Color.CornflowerBlue, Bold = true },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = Color.LightGreen, Bold = true },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = Color.LightGray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = Color.DeepSkyBlue },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = Color.Black, Background = Color.Yellow },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = Color.White, Background = Color.Red },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = Color.White, Background = Color.MediumVioletRed, Bold = true },
        });


    public static LINQPadTheme LINQPadLiterate { get; } = new(
        new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
            [ConsoleThemeStyle.Text] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.Invalid] = new() { Background = Color.Yellow, Italic = true },
            [ConsoleThemeStyle.Null] = new() { Foreground = Color.Blue },
            [ConsoleThemeStyle.Name] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.String] = new() { Foreground = Color.DarkCyan, Bold = true },
            [ConsoleThemeStyle.Number] = new() { Foreground = Color.Magenta, Bold = true },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = Color.Blue, Bold = true },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = Color.Green, Bold = true },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = Color.LightGray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = Color.Black, Background = Color.Yellow },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = Color.White, Background = Color.Red },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = Color.White, Background = Color.Red, Bold = true },
        });


    public static LINQPadTheme LINQPadColored { get; } = new(
        new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
            [ConsoleThemeStyle.Text] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.Invalid] = new() { Foreground = Color.Yellow },
            [ConsoleThemeStyle.Null] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.Name] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.String] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.Number] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = Color.Black },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = Color.Gray, Background = Color.DarkGray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = Color.Black, Background = Color.DarkGray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = Color.Black, Background = Color.Blue },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = Color.DarkGray, Background = Color.Yellow },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = Color.Black, Background = Color.Red },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = Color.Black, Background = Color.Red },
        });


    public static LINQPadTheme Literate { get; } = new(
        new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
            [ConsoleThemeStyle.Text] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.Invalid] = new() { Foreground = Color.Yellow },
            [ConsoleThemeStyle.Null] = new() { Foreground = Color.Blue },
            [ConsoleThemeStyle.Name] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.String] = new() { Foreground = Color.Cyan },
            [ConsoleThemeStyle.Number] = new() { Foreground = Color.Magenta },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = Color.Blue },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = Color.Green },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = Color.Yellow },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = Color.White, Background = Color.Red },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = Color.White, Background = Color.Red },
        });


    public static LINQPadTheme Grayscale { get; } = new(
        new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
            [ConsoleThemeStyle.Text] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.Invalid] = new() { Foreground = Color.White, Background = Color.DarkGray },
            [ConsoleThemeStyle.Null] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Name] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.String] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Number] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = Color.White, Background = Color.DarkGray },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = Color.Black, Background = Color.White },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = Color.Black, Background = Color.White },
        });


    public static LINQPadTheme Colored { get; } = new(
        new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
            [ConsoleThemeStyle.Text] = new() { Foreground = Color.Gray },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = Color.DarkGray },
            [ConsoleThemeStyle.Invalid] = new() { Foreground = Color.Yellow },
            [ConsoleThemeStyle.Null] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Name] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.String] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Number] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = Color.White },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = Color.Gray, Background = Color.DarkGray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = Color.White, Background = Color.DarkGray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = Color.White, Background = Color.Blue },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = Color.DarkGray, Background = Color.Yellow },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = Color.White, Background = Color.Red },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = Color.White, Background = Color.Red },
        });
}