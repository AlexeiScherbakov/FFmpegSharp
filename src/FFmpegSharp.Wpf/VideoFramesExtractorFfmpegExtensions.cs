using System;
using System.Windows.Media.Imaging;

using FFmpegSharp.CommandLine;

namespace FFmpegSharp.Wpf
{
	public static class VideoFramesExtractorFfmpegExtensions
	{
		/// <summary>
		/// Returns video frame in <see cref="BitmapImage"/> from file <paramref name="fileName"/> at time <paramref name="frameTime"/>
		/// </summary>
		/// <param name="extractor">frame extractor</param>
		/// <param name="fileName">video fileName</param>
		/// <param name="frameTime">time of video frame</param>
		/// <returns></returns>
		public static FFmpegCommandLineResult<BitmapImage> GetFrameBitmapImageForVideoFile(
			this VideoFramesExtractorFfmpeg extractor,
			string fileName,
			TimeSpan frameTime,
			bool freeze = true
			)
		{
			var cmdResult = extractor.GetFrameStreamForVideoFile(fileName, frameTime);

			return cmdResult.Convert(x =>
			{
				if (x is null)
				{
					return null;
				}
				BitmapImage bitmap = new BitmapImage();
				bitmap.BeginInit();
				x.Position = 0;
				bitmap.StreamSource = x;
				bitmap.EndInit();
				if (freeze)
				{
					bitmap.Freeze();
				}
				return bitmap;
			});
		}
	}
}
