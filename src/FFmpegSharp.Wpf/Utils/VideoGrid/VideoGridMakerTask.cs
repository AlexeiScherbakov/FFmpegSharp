using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using FFmpegSharp.CommandLine;

namespace FFmpegSharp.Wpf.Utils.VideoGrid
{

	internal sealed class VideoGridMakerTask
	{
		private Guid _uid;
		private string _fileName;
		private Stream _outputStream;
		private VideoGridParameters _parameters;

		private BitmapImage[,] _grid;
		private TimeSpan[,] _times;

		private int _state;

		private int _xSize;

		private int _currentY = 0;

		private OutputFormat _outputFormat;

		private WriteableBitmap _bitmap;

		public VideoGridMakerTask(string fileName, VideoGridParameters parameters, Stream outputStream, OutputFormat outputFormat)
		{
			_uid = Guid.NewGuid();

			_fileName = fileName;
			_parameters = parameters;
			_outputStream = outputStream;
			_outputFormat = outputFormat;

			_grid = new BitmapImage[parameters.XCount, parameters.YCount];
			_times = new TimeSpan[parameters.XCount, parameters.YCount];
		}

		public Guid Uid
		{
			get { return _uid; }
		}

		public int State
		{
			get { return _state; }
		}

		/// <summary>
		/// Эта функция загружает 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		internal bool Load(FFmpegContext context)
		{
			var info = context.GetVideoInformationExtractor();
			var durationResult = info.GetContainerDuration(_fileName);
			if (durationResult.Result != null)
			{
				return false;
			}

			int count = _parameters.XCount * _parameters.YCount + 1;
			var duration = durationResult.Result.Value;
			var timePart = TimeSpan.FromSeconds((duration.TotalSeconds / count));


			TimeSpan time = TimeSpan.Zero;
			var frameExtractor = context.GetVideoFramesExtractor();
			for (int y = 0; y < _parameters.YCount; y++)
			{
				for (int x = 0; x < _parameters.XCount; x++)
				{
					time += timePart;
					var frameResult = frameExtractor.GetFrameBitmapImageForVideoFile(_fileName, time);
					_times[x, y] = time;
					_grid[x, y] = frameResult.Result;
				}
			}

			return true;
		}

		internal void Make()
		{
			WriteableBitmap bitmap = new WriteableBitmap(_parameters.Width, _parameters.Height, 96.0, 96.0, PixelFormats.Bgr32, null);


			for (int y = 0; y < _parameters.YCount; y++)
			{
				for (int x = 0; x < _parameters.XCount; x++)
				{
					var scaledBitmap = ScaleBitmap(_grid[x, y]);
				}
			}
			_state = 1;
		}

		private void WriteBitmapToZone(BitmapSource src, int x, int y)
		{
			var f = new FormatConvertedBitmap();
			f.BeginInit();
			f.Source = src;
			f.DestinationFormat = PixelFormats.Bgra32;
			f.EndInit();
			var bSrc = new WriteableBitmap(f);
		}

		private BitmapSource ScaleBitmap(BitmapSource src)
		{
			//var x = src.PixelWidth;
			//var y = src.PixelHeight;

			//if (_parameters.SizeMode == GridSizeMode.FixedWidth)
			//{
			//	if (x<=)
			//}

			//if ((x <= _xSize) && (y <= _ySize))
			//{
			//	return src;
			//}

			//double xScale = ((double) _xSize) / x;
			//double yScale = ((double) _ySize) / y;

			//double scale = Math.Min(xScale, yScale);

			//ScaleTransform transform = new ScaleTransform(scale, scale);
			//TransformedBitmap bitmap = new TransformedBitmap(src, transform);
			//return bitmap;
			return null;
		}


		public void Save()
		{
			BitmapEncoder encoder = null;
			switch (_outputFormat)
			{
				case OutputFormat.Png:
					encoder = new PngBitmapEncoder()
					{

					};
					break;
				case OutputFormat.Jpg:
					encoder = new JpegBitmapEncoder()
					{

					};
					break;
				case OutputFormat.Bmp:
					encoder = new BmpBitmapEncoder()
					{

					};
					break;
			}
			encoder.Frames.Add(BitmapFrame.Create(_bitmap));
			encoder.Save(_outputStream);
			_outputStream.Flush();
			_outputStream.Close();
		}
	}
}
