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
using System.IO;
using System.Linq;


namespace Serilog.Sinks.LINQPad.Themes
{

    public class LINQPadTheme : ConsoleTheme
    {
        /// <summary>
        /// Construct a theme given a set of styles.
        /// </summary>
        /// <param name="styles">Styles to apply within the theme.</param>
        public LINQPadTheme(IReadOnlyDictionary<ConsoleThemeStyle, LINQPadThemeStyle> styles)
        {
            if (styles == null) {
                throw new ArgumentNullException(nameof(styles));
            }

            Styles = styles.ToDictionary(kv => kv.Key, kv => kv.Value);
        }


        /// <inheritdoc/>
        public IReadOnlyDictionary<ConsoleThemeStyle, LINQPadThemeStyle> Styles { get; }

        /// <inheritdoc/>
        public override bool CanBuffer => false;

        /// <inheritdoc/>
        protected override int ResetCharCount { get; }


        /// <inheritdoc/>
        public override int Set(TextWriter output, ConsoleThemeStyle style)
        {
            if (Styles.TryGetValue(style, out var wcts)) {
                NextColors = wcts;
            }

            return 0;
        }


        /// <inheritdoc/>
        public override void Reset(TextWriter output)
            => NextColors = LINQPadThemeStyle.None;


        public override void ApplyColors(TextWriter output)
        {
            if (!CurrColors.Equals(NextColors)) {
                if (SpanDepth > 0) {
                    output.Write("</span>");
                    SpanDepth--;
                }

                var declarations = new[] {
                    NextColors.Foreground.HasValue ? $"color:{ColorTranslator.ToHtml(NextColors.Foreground.Value)}" : null,
                    NextColors.Background.HasValue ? $"background-color:{ColorTranslator.ToHtml(NextColors.Background.Value)}" : null,
                    NextColors.Bold ? "font-weight:bold" : null,
                    NextColors.Italic ? "font-style:italic" : null
                };
                var style = String.Join("; ", declarations.Where(x => x != null));
                if (style.Length > 0) {
                    output.Write($"<span style='{style}'>");
                    CurrColors = NextColors;
                    SpanDepth++;
                }
            }
        }


        protected LINQPadThemeStyle CurrColors;
        protected LINQPadThemeStyle NextColors;
        protected int SpanDepth = 0;
    }

}
