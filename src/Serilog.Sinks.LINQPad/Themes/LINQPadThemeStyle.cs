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
using System.Drawing;


namespace Serilog.Sinks.LINQPad.Themes
{

    /// <summary>
    /// Styling applied using the <see cref="System.ConsoleColor"/> enumeration.
    /// </summary>
    public struct LINQPadThemeStyle
    {
        /// <summary>
        /// The foreground color to apply.
        /// </summary>
        public Color? Foreground;

        /// <summary>
        /// The background color to apply.
        /// </summary>
        public Color? Background;

        public bool Bold;
        public bool Italic;


        public LINQPadThemeStyle(Color? foreground = null, Color? background = null, bool? bold = null, bool? italic = null)
        {
            Foreground = foreground;
            Background = background;
            Bold = bold.HasValue && bold.Value;
            Italic = italic.HasValue && italic.Value;
        }


        public static LINQPadThemeStyle None = new LINQPadThemeStyle();
    }

}
