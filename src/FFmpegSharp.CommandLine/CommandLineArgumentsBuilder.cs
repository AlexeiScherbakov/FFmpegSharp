using System.Text;

namespace FFmpegSharp.CommandLine
{
	/// <summary>
	/// Low level commandline builder
	/// </summary>
	internal sealed class CommandLineArgumentsBuilder
	{
		private StringBuilder _stringBuilder;
		private bool _notEmpty;

		public CommandLineArgumentsBuilder()
		{
			_stringBuilder = new StringBuilder(256);
			_notEmpty = false;
		}

		public void AppendArgument(string arg)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append(arg);
			_notEmpty = true;
		}

		public void AppendArguments(string arg0,string arg1)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append(arg0);
			_notEmpty = true;
			_stringBuilder.Append(' ');
			_stringBuilder.Append(arg1);
		}

		public void AppendArguments(string arg0, string arg1,string arg2)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append(arg0);
			_notEmpty = true;
			_stringBuilder.Append(' ');
			_stringBuilder.Append(arg1);
			_stringBuilder.Append(' ');
			_stringBuilder.Append(arg2);
		}

		public void AppendArguments(string arg0, string arg1, string arg2,string arg3)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append(arg0);
			_notEmpty = true;
			_stringBuilder.Append(' ');
			_stringBuilder.Append(arg1);
			_stringBuilder.Append(' ');
			_stringBuilder.Append(arg2);
			_stringBuilder.Append(' ');
			_stringBuilder.Append(arg3);
		}

		public void AppendArguments(params string[] args)
		{
			if (args is null)return;
			if (args.Length == 0) return;

			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append(args[0]);
			_notEmpty = true;
			for (int i = 1; i < args.Length; i++)
			{
				_stringBuilder.Append(' ');
				_stringBuilder.Append(args[i]);
			}
		}

		public void AppendArgumentFormat(string format, object arg0)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.AppendFormat(format, arg0);
			_notEmpty = true;
		}

		public void AppendArgumentInDoubleQuotes(string arg)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append('"');
			_stringBuilder.Append(arg);
			_stringBuilder.Append('"');
			_notEmpty = true;
		}

		public void AppedArgumentInSingleQuotes(string arg)
		{
			if (_notEmpty)
			{
				_stringBuilder.Append(' ');
			}
			_stringBuilder.Append('\'');
			_stringBuilder.Append(arg);
			_stringBuilder.Append('\'');
			_notEmpty = true;
		}


		public override string ToString()
		{
			return _stringBuilder.ToString();
		}
	}
}
