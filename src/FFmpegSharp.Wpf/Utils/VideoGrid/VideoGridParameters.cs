namespace FFmpegSharp.Wpf
{


	public sealed class VideoGridParameters
	{
		private string _customFileName;
		private int _xCount;
		private int _yCount;
		private int _gridElementMargin;

		public string CustomFileName
		{
			get { return _customFileName; }
			set { _customFileName = value; }
		}

		public int XCount
		{
			get { return _xCount; }
			set { _xCount = value; }
		}

		public int YCount
		{
			get { return _yCount; }
			set { _yCount = value; }
		}

		public int GridElementMargin
		{
			get { return _gridElementMargin; }
			set { _gridElementMargin = value; }
		}

		public int Width { get; set; }
		public int Height { get; set; }
	}
}
