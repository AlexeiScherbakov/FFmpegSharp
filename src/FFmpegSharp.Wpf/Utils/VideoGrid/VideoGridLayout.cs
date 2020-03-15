using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FFmpegSharp.Wpf.Utils.VideoGrid
{
	public class VideoGridLayout
	{
		private int _width;
		private int _height;

		private List<VideoGridLayoutElement> _elements = new List<VideoGridLayoutElement>();

		public List<VideoGridLayoutElement> Elements
		{
			get { return _elements; }
		}

		public int Width
		{
			get { return _width; }
			set { _width = value; }
		}
	}


	public class VideoGridLayoutElement
	{
		private int _left;
		private int _top;

		public int Left
		{
			get { return _left; }
			set { _left = value; }
		}

		public int Top
		{
			get { return _top; }
			set { _top = value; }
		}
	}

	public class VideoGridBaseTextElement
		: VideoGridLayoutElement
	{

	}

	public class VideoGridSpecialTextElement
		:VideoGridBaseTextElement
	{
		private SpecialTextElement _text;

		public SpecialTextElement Text
		{
			get { return _text; }
			set { _text = value; }
		}
	}

	public class VideoGridTextElement
		:VideoGridBaseTextElement
	{
		
	}

	public class VideoGridBaseContentElement
		: VideoGridLayoutElement
	{
		private int _width;
		private int _height;

		public int Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public int Height
		{
			get { return _height; }
			set { _height = value; }
		}
	}

	public class VideoGridImageElement
		:VideoGridBaseContentElement
	{
		private ImageSource _imageSource;

		public ImageSource ImageSource
		{
			get { return _imageSource; }
			set { _imageSource = value; }
		}

		public static VideoGridImageElement CreateFromBitmapSource(BitmapSource bitmapSource)
		{
			VideoGridImageElement element = new VideoGridImageElement();
			element.Width = bitmapSource.PixelWidth;
			element.Height = bitmapSource.PixelHeight;
			element._imageSource = bitmapSource;
			return element;
		}
	}


	public enum SpecialTextElement
	{
		None,
	}
}
