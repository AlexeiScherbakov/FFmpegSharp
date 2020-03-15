using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

using FFmpegSharp.CommandLine;

namespace FFmpegSharp.Wpf.Utils.VideoGrid
{
	public class VideoGridMaker
	{
		private FFmpegContext _context;

		private BlockingCollection<VideoGridMakerTask> _ioQueue = new BlockingCollection<VideoGridMakerTask>(
			new ConcurrentQueue<VideoGridMakerTask>());

		private BlockingCollection<VideoGridMakerTask> _processQueue = new BlockingCollection<VideoGridMakerTask>(
			new ConcurrentQueue<VideoGridMakerTask>());


		private CancellationTokenSource _cancel;

		private Thread _ioThread;
		private Thread _makeThread;

		public VideoGridMaker(FFmpegContext context)
		{
			_context = context;

			_ioThread = new Thread(IoThread)
			{
				IsBackground = true
			};
			_ioThread.Start();

			_makeThread = new Thread(MakeThread)
			{
				IsBackground = true
			};
			_makeThread.Start();
		}


		public Guid QueueFile(string fileName,VideoGridParameters parameters,VideoGridLayout layout, string outputFileName)
		{
			var fileStream = new FileStream(outputFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			VideoGridMakerTask task = new VideoGridMakerTask(fileName, parameters, fileStream, OutputFormat.Jpg);

			_ioQueue.Add(task);

			return task.Uid;
		}

		/// <summary>
		/// Load+Save
		/// </summary>
		private void IoThread()
		{
			VideoGridMakerTask task;
			if (_ioQueue.TryTake(out task,-1, _cancel.Token))
			{
				if (_cancel.IsCancellationRequested)
				{
					return;
				}
				if (task.State == 0)
				{
					// загрузка
					task.Load(_context);
					_processQueue.Add(task);
				}
				else
				{
					// сохранение
					task.Save();

					var evnt = TaskCompleted;
					if (evnt != null)
					{
						evnt(task.Uid);
					}
				}


				
			}
		}


		/// <summary>
		/// Make operations
		/// </summary>
		private void MakeThread()
		{
			VideoGridMakerTask task;
			if (_processQueue.TryTake(out task,-1,_cancel.Token))
			{
				if (_cancel.IsCancellationRequested)
				{
					return;
				}
				task.Make();

				_ioQueue.Add(task);
			}
		}


		public Action<Guid> TaskCompleted;
	}
}
