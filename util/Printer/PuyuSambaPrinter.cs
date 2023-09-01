using System.Timers;
using puka;

namespace ESCPOS_NET
{
	public class PuyuSambaPrinter : BasePrinter
	{
		private string _filePath;
		private string _tempFileBasePath;
		private string? _tempFilePath;
		private MemoryStream _stream;

		public PuyuSambaPrinter(string tempFileBasePath, string filePath) : base()
		{
			_tempFileBasePath = tempFileBasePath;
			if (!Directory.Exists(_tempFileBasePath))
			{
				Directory.CreateDirectory(_tempFileBasePath);
			}
			_stream = new MemoryStream();
			Writer = new BinaryWriter(_stream);
			_filePath = filePath;
		}

		public bool GetOnlineStatus()
		{
			try
			{
				_tempFilePath = Path.Combine(_tempFileBasePath, $"{Guid.NewGuid()}.bin");
				File.WriteAllText(_tempFilePath, "");
				var task = new Task<bool>(() =>
				{
					try
					{
						File.Copy(_tempFilePath, _filePath);
						return true;
					}
					catch (System.Exception)
					{
						return false;
					}
					finally
					{
						File.Delete(_tempFilePath);
					}
				});
				task.Start();
				return task.Wait(1500) && task.Result;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		public override void Flush(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (BytesWrittenSinceLastFlush > 0)
				{
					var bytes = _stream.ToArray();
					_stream = new MemoryStream();
					Writer = new BinaryWriter(_stream);

					_tempFilePath = Path.Combine(_tempFileBasePath, $"{Guid.NewGuid()}.bin");
					File.WriteAllBytes(_tempFilePath, bytes);
					File.Copy(_tempFilePath, _filePath);
					File.Delete(_tempFilePath);

				}
				BytesWrittenSinceLastFlush = 0;
			}
			catch (System.Exception ex)
			{
				Program.Logger.Error("Error al limpiar SambaPrinter", ex.Message);
			}
		}
	}
}