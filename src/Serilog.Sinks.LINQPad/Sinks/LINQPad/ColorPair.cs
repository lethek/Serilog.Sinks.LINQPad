using System.Drawing;


namespace Serilog.Sinks.LINQPad
{

	/// <summary>
	/// Use to specify a Foreground-Background color pair
	/// </summary>
	public class ColorPair
	{
		/// <summary>
		/// Set the foreground color; setting it to null should indicate a "default" or "transparent" color
		/// </summary>
		public Color? Foreground { get; set; }

		/// <summary>
		/// Set the background color; setting it to null should indicate a "default" or "transparent" color
		/// </summary>
		public Color? Background { get; set; }


		/// <summary>
		/// Initialize a foreground-background color pair
		/// </summary>
		/// <param name="foreground">Set the foreground color; setting it to null should indicate a "default" or "transparent" color</param>
		/// <param name="background">Set the background color; setting it to null should indicate a "default" or "transparent" color</param>
		public ColorPair(Color? foreground = null, Color? background = null)
		{
			Foreground = foreground;
			Background = background;
		}
	}

}