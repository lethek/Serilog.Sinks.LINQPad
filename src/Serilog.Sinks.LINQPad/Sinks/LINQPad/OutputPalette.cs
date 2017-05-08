using System.Drawing;


namespace Serilog.Sinks.LINQPad
{

	/// <summary>
	/// Defines a customizable color-palette for use by the Serilog LINQPad sink.
	/// The default colors are intended for use on a white background.
	/// </summary>
	public class OutputPalette
	{
		/// <summary>
		/// Default: new ColorPair(Color.Black)
		/// </summary>
		public ColorPair Text { get; set; } = new ColorPair(Color.Black);
		/// <summary>
		/// Default: new ColorPair(Color.Gray)
		/// </summary>
		public ColorPair Subtext { get; set; } = new ColorPair(Color.Gray);
		/// <summary>
		/// Default: new ColorPair(Color.DarkGray)
		/// </summary>
		public ColorPair Punctuation { get; set; } = new ColorPair(Color.DarkGray);

		/// <summary>
		/// Default: new ColorPair(Color.Gray)
		/// </summary>
		public ColorPair VerboseLevel { get; set; } = new ColorPair(Color.Gray);
		/// <summary>
		/// Default: new ColorPair(Color.Gray)
		/// </summary>
		public ColorPair DebugLevel { get; set; } = new ColorPair(Color.Gray);
		/// <summary>
		/// Default: new ColorPair(Color.Black)
		/// </summary>
		public ColorPair InformationLevel { get; set; } = new ColorPair(Color.Black);
		/// <summary>
		/// Default: new ColorPair(Color.Black, Color.Yellow)
		/// </summary>
		public ColorPair WarningLevel { get; set; } = new ColorPair(Color.Black, Color.Yellow);
		/// <summary>
		/// Default: new ColorPair(Color.White, Color.Red)
		/// </summary>
		public ColorPair ErrorLevel { get; set; } = new ColorPair(Color.White, Color.Red);
		/// <summary>
		/// Default: new ColorPair(Color.White, Color.Red)
		/// </summary>
		public ColorPair FatalLevel { get; set; } = new ColorPair(Color.White, Color.Red);

		/// <summary>
		/// Default: new ColorPair(Color.Blue)
		/// </summary>
		public ColorPair KeywordSymbol { get; set; } = new ColorPair(Color.Blue);
		/// <summary>
		/// Default: new ColorPair(Color.Magenta)
		/// </summary>
		public ColorPair NumericSymbol { get; set; } = new ColorPair(Color.Magenta);
		/// <summary>
		/// Default: new ColorPair(Color.DarkCyan)
		/// </summary>
		public ColorPair StringSymbol { get; set; } = new ColorPair(Color.DarkCyan);
		/// <summary>
		/// Default: new ColorPair(Color.Green)
		/// </summary>
		public ColorPair OtherSymbol { get; set; } = new ColorPair(Color.Green);
		/// <summary>
		/// Default: new ColorPair(Color.Gray)
		/// </summary>
		public ColorPair NameSymbol { get; set; } = new ColorPair(Color.Gray);
		/// <summary>
		/// Default: new ColorPair(Color.Orange)
		/// </summary>
		public ColorPair RawText { get; set; } = new ColorPair(Color.Orange);
	}

}