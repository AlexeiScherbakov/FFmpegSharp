using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegSharp.CommandLine
{
	/// <summary>
	/// Execution result
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class FFmpegCommandLineResult<TResult>
	{
		private readonly TResult _result;
		private readonly string _errorText;

		/// <summary>
		/// ctor
		/// </summary>
		public FFmpegCommandLineResult(TResult result,string errorText)
		{
			_result = result;
			_errorText = errorText;
		}

		/// <summary>
		/// Result object
		/// </summary>
		public TResult Result
		{
			get { return _result; }
		}

		/// <summary>
		/// Error text from StandardError
		/// </summary>
		public string ErrorText
		{
			get { return _errorText; }
		}


		public FFmpegCommandLineResult<WResult> Convert<WResult>(Func<TResult,WResult> converter)
		{
			var convertedResult = converter(_result);
			return new FFmpegCommandLineResult<WResult>(convertedResult, _errorText);
		}
	}
}
