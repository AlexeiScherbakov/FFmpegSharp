using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace FFmpegSharp.CommandLine
{
	/// <summary>
	/// ffprobe utility
	/// </summary>
	public class VideoInformationExtractorFfprobe
	{
		private readonly string _app;

		internal VideoInformationExtractorFfprobe(string app)
		{
			_app = app;
		}

		/// <summary>
		/// Get duration of video container
		/// </summary>
		/// <param name="fileName">video fileName</param>
		/// <returns></returns>
		public FFmpegCommandLineResult<TimeSpan?> GetContainerDuration(string fileName)
		{
			//ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 input.mp4
			CommandLineArgumentsBuilder args = new CommandLineArgumentsBuilder();
			args.AppendArguments("-v error", "-show_entries", "format=duration -of default=noprint_wrappers=1:nokey=1");
			args.AppendArgumentInDoubleQuotes(fileName);

			var psi = StdioUtils.CreateBaseStartInfo(_app);
			psi.Arguments = args.ToString();

			var process = new Process();
			process.StartInfo = psi;
			process.Start();

			process.WaitForExit(1000);
			var result = process.StandardOutput.ReadToEnd().Trim(' ', '\r', '\n');
			var error = process.StandardError.ReadToEnd();

			double seconds;
			if (double.TryParse(result,
				NumberStyles.Float,
				CultureInfo.InvariantCulture,
				out seconds))
			{
				TimeSpan ret = TimeSpan.FromSeconds(seconds);
				return new FFmpegCommandLineResult<TimeSpan?>(ret, error);
			}
			else
			{
				return new FFmpegCommandLineResult<TimeSpan?>(null, error);
			}	
		}
	}
}
