using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using printer_aplication_desktop.utils;
using System.Globalization;
using System.Text;

namespace printer_aplication_desktop.components
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

		private string RemoveDiacritics(string input)
		{
			string normalizedString = input.Normalize(NormalizationForm.FormD);
			string result = new string(normalizedString
					.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					.ToArray());

			return result;
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
					printer = new SambaPrinter(hostname.ToString(), port.ToString());
					break;
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
			string textReplaced = RemoveDiacritics(textPrinter);

			byte[] elementText = CombinePrinterParameter(epsonPrinter.PrintLine(textReplaced));

			return elementText;
		}

		public byte[] PrintData(string textPrinter)
		{
			string textReplaced = RemoveDiacritics(textPrinter);

			byte[] elementText = CombinePrinterParameter(epsonPrinter.Print(textReplaced));

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

		public string PadRightText(string text, int width, char characterPad)
		{
			int totalPadding = width - text.Length;

			string paddedLeft = text + new string(characterPad, totalPadding);

			return paddedLeft;
		}

		public string UFTCharacter(string str)
		{
			UTF8Encoding utf8 = new UTF8Encoding();

			Byte[] encodedBytes = utf8.GetBytes(str);
			String decodedString = utf8.GetString(encodedBytes);

			return decodedString;
		}
	}
}
