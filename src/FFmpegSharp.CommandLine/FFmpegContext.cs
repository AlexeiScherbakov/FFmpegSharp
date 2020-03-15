using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FFmpegSharp.CommandLine
{
	/// <summary>
	/// Represents locations of FFmpeg suite programs
	/// </summary>
	public sealed class FFmpegContext
	{
		private readonly string _ffmpegLocation;
		private readonly string _ffplayLocation;
		private readonly string _ffprobeLocation;

		public FFmpegContext(string directory)
		{
			if (!Directory.Exists(directory))
			{
				throw new ArgumentException("Directory is not exists", "directory");
			}

			var ffmpeg = Path.Combine(directory, "ffmpeg*");

			var files = Directory.GetFiles(directory, "ffmpeg*", SearchOption.TopDirectoryOnly);

			if (files.Length == 1)
			{
				_ffmpegLocation = files[0];
			}
			files = Directory.GetFiles(directory, "ffplay*", SearchOption.TopDirectoryOnly);
			if (files.Length == 1)
			{
				_ffplayLocation = files[0];
			}
			files = Directory.GetFiles(directory, "ffprobe*", SearchOption.TopDirectoryOnly);
			if (files.Length == 1)
			{
				_ffprobeLocation = files[0];
			}
			if ((null == _ffmpegLocation) && (null == _ffplayLocation) && (null == _ffprobeLocation))
			{
				throw new ArgumentException("This is not ffmpeg directory", "directory");
			}
		}

		public FFmpegContext(string ffmpeg,string ffplay,string ffprobe)
		{
			if ((!string.IsNullOrEmpty(ffmpeg)) && File.Exists(ffmpeg))
			{
				_ffmpegLocation = ffmpeg;
			}
			if ((!string.IsNullOrEmpty(ffplay)) && File.Exists(ffplay))
			{
				_ffplayLocation = ffplay;
			}
			if ((!string.IsNullOrEmpty(ffprobe)) && File.Exists(ffprobe))
			{
				_ffprobeLocation = ffprobe;
			}
			if ((null == _ffmpegLocation) && (null == _ffplayLocation) && (null == _ffprobeLocation))
			{
				throw new ArgumentException("At least one command line utility location must be specified");
			}
		}


		public string FfmpegLocation
		{
			get { return _ffmpegLocation; }
		}

		public string FfplayLocation
		{
			get { return _ffplayLocation; }
		}

		public string FfprobeLocation
		{
			get { return _ffprobeLocation; }
		}


		public VideoFramesExtractorFfmpeg GetVideoFramesExtractor()
		{
			return new VideoFramesExtractorFfmpeg(_ffmpegLocation);
		}

		public VideoInformationExtractorFfprobe GetVideoInformationExtractor()
		{
			return new VideoInformationExtractorFfprobe(_ffprobeLocation);
		}
	}
}
