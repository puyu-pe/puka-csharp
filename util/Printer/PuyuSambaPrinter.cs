using System;
using System.IO;
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
				Program.Logger.Error("Error al limpiar SambaPrinter",ex.Message);
			}
		}
	}
}