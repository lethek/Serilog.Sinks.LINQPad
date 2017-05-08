using System.Drawing;


namespace Serilog.Sinks.LINQPad
{

	public class ColorPair
	{
		public Color? Foreground { get; set; }
		public Color? Background { get; set; }


		public ColorPair(Color? foreground = null, Color? background = null)
		{
			Foreground = foreground;
			Background = background;
		}
	}

}