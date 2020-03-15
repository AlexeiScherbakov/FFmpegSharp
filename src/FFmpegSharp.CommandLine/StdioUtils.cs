using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FFmpegSharp.CommandLine
{

	internal static class StdioUtils
	{
		internal static ProcessStartInfo CreateBaseStartInfo(string executable)
		{
			ProcessStartInfo psi = new ProcessStartInfo()
			{
				FileName = executable,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};
			return psi;
		}

		internal static MemoryStream ReadProcessStandardOutput(Process process,int millisecondTimeout=1000)
		{
			process.WaitForExit(millisecondTimeout);

			return ReadStreamReaderRaw(process.StandardOutput);
		}

		internal static MemoryStream ReadStreamReaderRaw(StreamReader reader)
		{
			byte[] buffer = new byte[1024];
			MemoryStream m = new MemoryStream();
			int count = 0;
			do
			{
				count = reader.BaseStream.Read(buffer, 0, 1024);
				m.Write(buffer, 0, count);
			}
			while (count > 0);
			return m;
		}
	}
}
