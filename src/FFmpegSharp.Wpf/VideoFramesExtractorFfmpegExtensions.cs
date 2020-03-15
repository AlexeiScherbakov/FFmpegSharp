using System;
using System.Windows.Media.Imaging;

using FFmpegSharp.CommandLine;

namespace FFmpegSharp.Wpf
{
	public static class VideoFramesExtractorFfmpegExtensions
	{
		public static FFmpegCommandLineResult<BitmapImage> GetFrameBitmapImageForVideoFile(
			this VideoFramesExtractorFfmpeg extractor,
			string fileName,
			TimeSpan frameTime
			)
		{
			var cmdResult = extractor.GetFrameStreamForVideoFile(fileName, frameTime);
			if (cmdResult.Result == null)
			{
				return new FFmpegCommandLineResult<BitmapImage>(null, cmdResult.ErrorText);
			}
			BitmapImage bitmap = new BitmapImage();
			bitmap.BeginInit();
			var stream = cmdResult.Result;
			stream.Position = 0;
			bitmap.StreamSource = stream;
			bitmap.EndInit();
			return new FFmpegCommandLineResult<BitmapImage>(bitmap, cmdResult.ErrorText);
		}
	}
}
