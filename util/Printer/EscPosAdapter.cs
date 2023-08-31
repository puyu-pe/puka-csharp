using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Printers;
using ESCPOS_NET.Utilities;
using System.Text.RegularExpressions;


namespace puka.util.printer
{
	public class EscPosAdapter : IPrinterEscPos
	{
		private EPSON epsonPrinter;
		private object? printer;

		public EscPosAdapter(object hostname, object port, TypeConnectionPrinter typeConnectionPrinter)
		{
			epsonPrinter = new EPSON();
			ConnectionPrinter(hostname, port, typeConnectionPrinter);
		}

		private void ConnectionPrinter(object hostname, object port, TypeConnectionPrinter typeConnectionPrinter)
		{
			switch (typeConnectionPrinter)
			{
				case TypeConnectionPrinter.ImmediateNetwork:
					printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings()
					{
						ConnectionString = $"{hostname}:{port}",
						PrinterName = "PrinterConnector"
					});
					break;
				case TypeConnectionPrinter.Serial:
					int Bound = Convert.ToInt32(port.ToString());
					printer = new SerialPrinter(hostname.ToString(), Bound);
					break;
				case TypeConnectionPrinter.File:
					printer = new FilePrinter(hostname.ToString(), true);
					break;
				case TypeConnectionPrinter.Samba:
					{
						string filePath = hostname.ToString() ?? "";
						printer = new PuyuSambaPrinter(@"C:\Temp", filePath);
						break;
					}
				case TypeConnectionPrinter.WindowsUsb:
					{
						List<DeviceDetails> usbDevices = DeviceFinder.GetDevices();//gets the usb devices connected to the pc
						string? name_system = hostname.ToString()?.Replace(" ", "");
						DeviceDetails? targetDevice = usbDevices.Find((item) =>
						{
							string? displayName = item.DisplayName?.Replace(" ", "");
							return displayName == name_system;
						});
						if (targetDevice == null)
							throw new Exception($"Impresora USB {name_system}, no encontrada");
						string? usbPort = targetDevice.DevicePath;
						if (usbPort == null)
							throw new Exception($"No se encontro un DevicePath: {targetDevice.DisplayName}");
						printer = new USBPrinter(usbPort);
						break;
					}
				default:
					throw new ArgumentException("Tipo de impresora no válido.");
			}
		}

		public async Task<bool> IsPrinterOnline()
		{
			try
			{
				if (printer is ImmediateNetworkPrinter printerNetwork)
				{
					return await printerNetwork.GetOnlineStatus(epsonPrinter);
				}
				if (printer is PuyuSambaPrinter puyuSambaPrinter)
				{
					return puyuSambaPrinter.GetOnlineStatus();
				}
				if (printer is BasePrinter basePrinter)
				{
					//basePrinter.GetStatus(), lanza una excepción por defecto, ver la documentación de la libreria
					//https://github.com/lukevp/ESC-POS-.NET/blob/master/ESCPOS_NET/Printers/BasePrinter.cs
					return true;
				}
			}
			catch (System.Exception)
			{
				return false;
			}
			return false;
		}

		public async Task Print(byte[] dataPrintElement)
		{
			try
			{
				if (printer is ImmediateNetworkPrinter printerNetWork)
				{
					await printerNetWork.WriteAsync(CombinePrinterParameter(dataPrintElement));
				}

				if (printer is BasePrinter basePrinter)
				{
					basePrinter.Write(CombinePrinterParameter(dataPrintElement));
					Thread.Sleep(500); // Importante, hasta que se libere la impresora
					basePrinter.Dispose();
				}
			}
			catch (System.Exception ex)
			{
				throw new Exception("Excepción en metodo Print(): " + ex.Message);
			}
		}

		public byte[] CombinePrinterParameter(params byte[][] dataPrinter)
		{
			byte[] builder = new byte[] { };

			foreach (var byteArray in dataPrinter)
			{
				builder = ByteSplicer.Combine(
						builder,
						byteArray);
			}
			return builder;
		}

		public byte[] DoubleHeightWeightText()
		{
			byte[] elementDouble = CombinePrinterParameter(epsonPrinter.SetStyles(PrintStyle.DoubleHeight | PrintStyle.DoubleWidth));
			return elementDouble;
		}

		public byte[] PrintDataLine(string textPrinter)
		{

			byte[] elementText = CombinePrinterParameter(epsonPrinter.PrintLine(textPrinter));

			return elementText;
		}

		public byte[] PrintData(string textPrinter)
		{

			byte[] elementText = CombinePrinterParameter(epsonPrinter.Print(textPrinter));

			return elementText;
		}

		public byte[] CenterTextPosition()
		{
			byte[] elementCenter = CombinePrinterParameter(epsonPrinter.CenterAlign());

			return elementCenter;
		}

		public byte[] LeftTextPosition()
		{
			byte[] elementLeft = CombinePrinterParameter(epsonPrinter.LeftAlign());

			return elementLeft;
		}

		public byte[] RightTextPosition()
		{
			byte[] elementRight = CombinePrinterParameter(epsonPrinter.RightAlign());

			return elementRight;
		}

		public byte[] BoldTextFont()
		{
			byte[] fontBold = CombinePrinterParameter(epsonPrinter.SetStyles(PrintStyle.Bold));

			return fontBold;
		}

		public byte[] FontBTextFont()
		{
			byte[] fontB = CombinePrinterParameter(epsonPrinter.SetStyles(PrintStyle.FontB));

			return fontB;
		}

		public byte[] NoneTextFont()
		{
			byte[] fontNone = CombinePrinterParameter(epsonPrinter.SetStyles(PrintStyle.None));

			return fontNone;
		}

		public byte[] PrintImageData(string imagePath)
		{
			byte[] dataImagePrinter = CombinePrinterParameter(
									epsonPrinter.PrintImage(File.ReadAllBytes(imagePath), true),
									PrintDataLine(""));

			return dataImagePrinter;
		}

		public byte[] TextInvertedFont(bool modeText)
		{
			byte[] textInvertedPrinter = CombinePrinterParameter(epsonPrinter.ReverseMode(modeText));

			return textInvertedPrinter;
		}

		public byte[] PrintQRCode(string dataQR)
		{
			byte[] dataQRPrinter = CombinePrinterParameter(epsonPrinter.PrintQRCode(dataQR));

			return dataQRPrinter;
		}

		public byte[] PrinterCutWidth(int quantity)
		{
			byte[] cutPrinter = CombinePrinterParameter(epsonPrinter.FullCutAfterFeed(quantity));

			return cutPrinter;
		}

		public string PadBoth(string str, int width, char paddingChar)
		{
			int totalPadding = width - str.Length;
			int leftPadding = totalPadding / 2;
			int rightPadding = totalPadding - leftPadding;

			if (str.Length > width)
			{
				leftPadding = 0;
				rightPadding = 0;
			}

			string paddedBoth = new string(paddingChar, leftPadding) + str + new string(paddingChar, rightPadding);

			return paddedBoth;
		}


		public byte[] EncodingLatin2()
		{
			return epsonPrinter.CodePage(CodePage.WPC1250_LATIN2);
		}

		public List<string> WrapText(string text, int charactersPerLine)
		{
			List<string> paragraph = new List<string>();
			MatchCollection matches = Regex.Matches(text, "." + "{1," + $"{charactersPerLine}" + "}");
			foreach (Match match in matches)
				paragraph.Add(match.Value);
			return paragraph;
		}
	}
}
