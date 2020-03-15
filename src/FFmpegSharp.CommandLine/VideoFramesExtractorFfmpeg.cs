using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FFmpegSharp.CommandLine
{
	public class VideoFramesExtractorFfmpeg
	{
		private readonly string _app;

		internal VideoFramesExtractorFfmpeg(string app)
		{
			_app = app;
		}

		public FFmpegCommandLineResult<MemoryStream> GetFrameStreamForVideoFile(string fileName, TimeSpan frameTime)
		{
			//ffmpeg -v quiet -ss 00:00:01.000 -i "filename" -an -frames:v 1 -f image2  pipe:
			//
			var args = new CommandLineArgumentsBuilder();
			args.AppendArguments("-v", "quiet", "-ss");
			args.AppendArgumentFormat(@"{0:hh\:mm\:ss\.fff}", frameTime);
			args.AppendArgument("-i");
			args.AppendArgumentInDoubleQuotes(fileName);
			args.AppendArguments("-an", "-frames:v 1", "-f image2", "pipe:");

			var psi = StdioUtils.CreateBaseStartInfo(_app);
			psi.Arguments = args.ToString();

			var process = new Process
			{
				StartInfo = psi
			};
			process.Start();


			var memoryStream = StdioUtils.ReadProcessStandardOutput(process);
			string errorText = process.StandardError.ReadToEnd();
			var ret = new FFmpegCommandLineResult<MemoryStream>(memoryStream, errorText);
			return ret;
		}

		public FFmpegCommandLineResult<byte[]> GetFrameDataForVideoFile(string fileName, TimeSpan frameTime)
		{
			var tmp = GetFrameStreamForVideoFile(fileName, frameTime);

			var ret = new FFmpegCommandLineResult<byte[]>((tmp.Result != null) ? tmp.Result.ToArray() : null, tmp.ErrorText);
			return ret;
		}
	}
}
