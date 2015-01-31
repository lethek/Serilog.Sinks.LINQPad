// Copyright 2015 Serilog Contributors
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

using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.LINQPad;

namespace Serilog
{

	/// <summary>
	/// Adds the WriteTo.LINQPad() extension method to <see cref="LoggerConfiguration"/>.
	/// </summary>
	public static class LoggerConfigurationLINQPadExtensions
	{

		/// <summary>
		/// Writes log events to the LINQPad Results panel, using color to differentiate between levels.
		/// </summary>
		/// <param name="sinkConfiguration">Logger sink configuration.</param>
		/// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink.</param>
		/// <param name="outputTemplate">A message template describing the format used to write to the sink. The default is "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}".</param>
		/// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
		/// <returns>Configuration object allowing method chaining.</returns>
		public static LoggerConfiguration LINQPad(
			this LoggerSinkConfiguration sinkConfiguration,
			LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
			string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
			IFormatProvider formatProvider = null)
		{
			if (sinkConfiguration == null) throw new ArgumentNullException("sinkConfiguration");
			if (outputTemplate == null) throw new ArgumentNullException("outputTemplate");
			return sinkConfiguration.Sink(new LINQPadSink(outputTemplate, formatProvider), restrictedToMinimumLevel);
		}

	}

}
