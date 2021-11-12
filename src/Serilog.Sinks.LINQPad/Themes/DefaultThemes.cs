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
using System.Collections.Generic;
using System.Drawing;


namespace Serilog.Sinks.LINQPad.Themes
{
    public static class DefaultThemes
    {
        /// <summary>
        /// No styling applied.
        /// </summary>
        public static ConsoleTheme None { get; } = new EmptyConsoleTheme();



        public static LINQPadTheme LINQPadDark { get; } = new LINQPadTheme(
            new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
                [ConsoleThemeStyle.Text] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.SecondaryText] = new LINQPadThemeStyle { Foreground = Color.LightGray },
                [ConsoleThemeStyle.TertiaryText] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.Invalid] = new LINQPadThemeStyle { Foreground = Color.Red, Background = Color.Yellow, Italic = true },
                [ConsoleThemeStyle.Null] = new LINQPadThemeStyle { Foreground = Color.Yellow, Italic = true },
                [ConsoleThemeStyle.Name] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.String] = new LINQPadThemeStyle { Foreground = Color.Cyan, Bold = true },
                [ConsoleThemeStyle.Number] = new LINQPadThemeStyle { Foreground = Color.Magenta, Bold = true },
                [ConsoleThemeStyle.Boolean] = new LINQPadThemeStyle { Foreground = Color.CornflowerBlue, Bold = true },
                [ConsoleThemeStyle.Scalar] = new LINQPadThemeStyle { Foreground = Color.LightGreen, Bold = true },
                [ConsoleThemeStyle.LevelVerbose] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.LevelDebug] = new LINQPadThemeStyle { Foreground = Color.LightGray },
                [ConsoleThemeStyle.LevelInformation] = new LINQPadThemeStyle { Foreground = Color.DeepSkyBlue },
                [ConsoleThemeStyle.LevelWarning] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.Yellow },
                [ConsoleThemeStyle.LevelError] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red },
                [ConsoleThemeStyle.LevelFatal] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.MediumVioletRed, Bold = true },
            });


        public static LINQPadTheme LINQPadLiterate { get; } = new LINQPadTheme(
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


        public static LINQPadTheme LINQPadColored { get; } = new LINQPadTheme(
            new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
                [ConsoleThemeStyle.Text] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.SecondaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.TertiaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.Invalid] = new LINQPadThemeStyle { Foreground = Color.Yellow },
                [ConsoleThemeStyle.Null] = new LINQPadThemeStyle { Foreground = Color.Black },
                [ConsoleThemeStyle.Name] = new LINQPadThemeStyle { Foreground = Color.Black },
                [ConsoleThemeStyle.String] = new LINQPadThemeStyle { Foreground = Color.Black },
                [ConsoleThemeStyle.Number] = new LINQPadThemeStyle { Foreground = Color.Black },
                [ConsoleThemeStyle.Boolean] = new LINQPadThemeStyle { Foreground = Color.Black },
                [ConsoleThemeStyle.Scalar] = new LINQPadThemeStyle { Foreground = Color.Black },
                [ConsoleThemeStyle.LevelVerbose] = new LINQPadThemeStyle { Foreground = Color.Gray, Background = Color.DarkGray },
                [ConsoleThemeStyle.LevelDebug] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.DarkGray },
                [ConsoleThemeStyle.LevelInformation] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.Blue },
                [ConsoleThemeStyle.LevelWarning] = new LINQPadThemeStyle { Foreground = Color.DarkGray, Background = Color.Yellow },
                [ConsoleThemeStyle.LevelError] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.Red },
                [ConsoleThemeStyle.LevelFatal] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.Red },
            });


        public static LINQPadTheme Literate { get; } = new LINQPadTheme(
            new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
                [ConsoleThemeStyle.Text] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.SecondaryText] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.TertiaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.Invalid] = new LINQPadThemeStyle { Foreground = Color.Yellow },
                [ConsoleThemeStyle.Null] = new LINQPadThemeStyle { Foreground = Color.Blue },
                [ConsoleThemeStyle.Name] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.String] = new LINQPadThemeStyle { Foreground = Color.Cyan },
                [ConsoleThemeStyle.Number] = new LINQPadThemeStyle { Foreground = Color.Magenta },
                [ConsoleThemeStyle.Boolean] = new LINQPadThemeStyle { Foreground = Color.Blue },
                [ConsoleThemeStyle.Scalar] = new LINQPadThemeStyle { Foreground = Color.Green },
                [ConsoleThemeStyle.LevelVerbose] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.LevelDebug] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.LevelInformation] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.LevelWarning] = new LINQPadThemeStyle { Foreground = Color.Yellow },
                [ConsoleThemeStyle.LevelError] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red },
                [ConsoleThemeStyle.LevelFatal] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red },
            });

        public static LINQPadTheme Grayscale { get; } = new LINQPadTheme(
            new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
                [ConsoleThemeStyle.Text] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.SecondaryText] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.TertiaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.Invalid] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.DarkGray },
                [ConsoleThemeStyle.Null] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Name] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.String] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Number] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Boolean] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Scalar] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.LevelVerbose] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.LevelDebug] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.LevelInformation] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.LevelWarning] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.DarkGray },
                [ConsoleThemeStyle.LevelError] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.White },
                [ConsoleThemeStyle.LevelFatal] = new LINQPadThemeStyle { Foreground = Color.Black, Background = Color.White },
            });

        public static LINQPadTheme Colored { get; } = new LINQPadTheme(
            new Dictionary<ConsoleThemeStyle, LINQPadThemeStyle> {
                [ConsoleThemeStyle.Text] = new LINQPadThemeStyle { Foreground = Color.Gray },
                [ConsoleThemeStyle.SecondaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.TertiaryText] = new LINQPadThemeStyle { Foreground = Color.DarkGray },
                [ConsoleThemeStyle.Invalid] = new LINQPadThemeStyle { Foreground = Color.Yellow },
                [ConsoleThemeStyle.Null] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Name] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.String] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Number] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Boolean] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.Scalar] = new LINQPadThemeStyle { Foreground = Color.White },
                [ConsoleThemeStyle.LevelVerbose] = new LINQPadThemeStyle { Foreground = Color.Gray, Background = Color.DarkGray },
                [ConsoleThemeStyle.LevelDebug] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.DarkGray },
                [ConsoleThemeStyle.LevelInformation] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Blue },
                [ConsoleThemeStyle.LevelWarning] = new LINQPadThemeStyle { Foreground = Color.DarkGray, Background = Color.Yellow },
                [ConsoleThemeStyle.LevelError] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red },
                [ConsoleThemeStyle.LevelFatal] = new LINQPadThemeStyle { Foreground = Color.White, Background = Color.Red },
            });
    }
}
