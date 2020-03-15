using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFmpegSharp.CommandLine
{
	public class FFmpegCommandLineResult<TResult>
	{
		private TResult _result;
		private string _errorText;

		public FFmpegCommandLineResult(TResult result,string errorText)
		{
			_result = result;
			_errorText = errorText;
		}


		public TResult Result
		{
			get { return _result; }
		}
		public string ErrorText
		{
			get { return _errorText; }
		}
	}
}
