﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using puka;


namespace puka.util.printer 
{
	public class EscPosClass
	{
		private int width = 42;
		private dynamic printer;
		private string type;
		private int times;
		private dynamic data;
		private IPrinterEscPos? connectorPrinter;

		public EscPosClass(dynamic data)
		{
			this.printer = data.printer;
			this.type = data.type;
			this.data = data.data;
			this.times = (data.times == null) ? 1 : data.times;
		}

		public async Task PrinterDocument()
		{
			try
			{
				ConnectTypePrinter();
				if (connectorPrinter == null)
					throw new Exception("connectorPrinter = null en PrinterDocument");
				if (!await connectorPrinter.IsPrinterOnline())
				{
					throw new Exception("La impresora no esta conectada");
				}
				for (int i = 0; i < times; i++)
				{
					await connectorPrinter.Print(PrintLayout());
				}
			}
			catch (System.Exception)
			{
				throw;
			}
		}

		private void ConnectTypePrinter()
		{
			try
			{
				switch (printer.type.ToString())
				{
					case "ethernet":
						connectorPrinter = new EscPosAdapter(printer.name_system, printer.port, TypeConnectionPrinter.ImmediateNetwork);
						break;
					case "linux-usb":
						connectorPrinter = new EscPosAdapter(printer.name_system, printer.port, TypeConnectionPrinter.File);
						break;
					case "serial":
						connectorPrinter = new EscPosAdapter(printer.name_system, printer.port, TypeConnectionPrinter.Serial);
						break;
					case "samba":
						connectorPrinter = new EscPosAdapter(printer.name_system, printer.port, TypeConnectionPrinter.Samba);
						break;
					default:
						throw new Exception("Tipo de impresora no soportado");
				}
			}
			catch (Exception ex)
			{
				Program.Logger.Warn(ex, "Error en ConnectTypePrinter", ex.Message);
			}
		}

		private byte[] PrintLayout()
		{
			byte[] result = Header();

			try
			{
				if (connectorPrinter == null)
					throw new Exception("connectorPrinter es null");
				switch (type)
				{
					case "invoice":
						result = connectorPrinter.CombinePrinterParameter(
								result,
								BusinessAdditional(),
								DocumentLegal(),
								Customer(),
								Additional(),
								Items(),
								Amounts(),
								AdditionalFooter(),
								FinalMessage(),
								StringQR(),
								connectorPrinter.PrinterCutWidth(2));
						break;

					case "note":
						result = connectorPrinter.CombinePrinterParameter(
								result,
								DocumentLegal(),
								Customer(),
								Additional(),
								connectorPrinter.PrinterCutWidth(2));
						break;

					case "command":
						result = connectorPrinter.CombinePrinterParameter(
								result,
								ProductionArea(),
								TextBackgroundInverted(),
								DocumentLegal(),
								Additional(),
								Items(),
								connectorPrinter.PrinterCutWidth(2));
						break;

					case "precount":
						result = connectorPrinter.CombinePrinterParameter(
								result,
								DocumentLegal(),
								Additional(),
								Items(),
								Amounts(),
								connectorPrinter.PrinterCutWidth(2));
						break;

					case "extra":
						result = connectorPrinter.CombinePrinterParameter(
								result,
								TitleExtra(),
								Additional(),
								Items(),
								Amounts(),
								connectorPrinter.PrinterCutWidth(2));
						break;

					default:
						throw new Exception("No se pudo conectar con la tiketera");
				}
			}
			catch (Exception ex)
			{
				Program.Logger.Warn(ex, "Ocurrio una excepcion en PrintLayout ", ex.Message);
			}
			return result;
		}

		private byte[] Header()
		{
			if (connectorPrinter == null)
				throw new Exception("Excepcion en la construcción de header, connectorPrinter null");
			byte[] result = connectorPrinter.CombinePrinterParameter(connectorPrinter.CenterTextPosition(),
					connectorPrinter.BoldTextFont());

			if (data.business.comercialDescription != null)
			{
				if (data.business.comercialDescription.type == "text")
				{
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.DoubleHeightWeightText(),
							connectorPrinter.PrintDataLine(data.business.comercialDescription.value.ToString().ToUpper()),
							connectorPrinter.NoneTextFont());
				}
				else if (data.business.comercialDescription.type == "img")
				{

					string imagePath = UserConfig.GetLogoPath();

					if (!File.Exists(imagePath))
					{
						throw new Exception("No se encontró el logo");
					}

					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.PrintImageData(imagePath),
							connectorPrinter.PrintDataLine("")
							);
				}
			}

			if (data.business.description != null)
			{
				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.PrintDataLine(data.business.description.ToString())
						);
			}

			return result;
		}

		private byte[] BusinessAdditional()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter igual a null en BusinessAdditional");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.CenterTextPosition(),
					connectorPrinter.NoneTextFont());

			if (data.business.additional == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			foreach (string additional in data.business.additional)
			{
				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.PrintDataLine(additional));
			}

			result = connectorPrinter.CombinePrinterParameter(
				 result,
				 connectorPrinter.PrintDataLine(""));

			return result;
		}

		private byte[] DocumentLegal()
		{
			if (connectorPrinter == null)
			{
				throw new Exception("connectorPrinter = null en DocumentLegal");
			}
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.NoneTextFont(),
					connectorPrinter.BoldTextFont());

			switch (type)
			{
				case "invoice":
				case "note":
				case "command":

					if (data.document != null)
					{
						string text = data.document.description.ToString() + " " + data.document.identifier.ToString();

						result = connectorPrinter.CombinePrinterParameter(
								result,
								connectorPrinter.CenterTextPosition(),
								connectorPrinter.PrintDataLine(text));
					}
					else
					{
						string text = data.document.ToString() + "  " + data.documentId.ToString();

						result = connectorPrinter.CombinePrinterParameter(
								result,
								connectorPrinter.CenterTextPosition(),
								connectorPrinter.PrintDataLine(text));
					}
					break;

				case "precount":
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.LeftTextPosition(),
							connectorPrinter.PrintDataLine(data.document.description.ToString()));
					break;
			}

			result = connectorPrinter.CombinePrinterParameter(
				 result,
				 connectorPrinter.PrintDataLine(""));

			return result;
		}

		private byte[] Customer()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en Customer");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.NoneTextFont(),
					connectorPrinter.LeftTextPosition());

			if (data.customer != null)
			{
				foreach (string row in data.customer)
				{
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.PrintDataLine(row));
				}
			}
			else
			{
				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.PrintDataLine(new string('-', width)));
			}

			return result;
		}

		private byte[] Additional()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en Additional");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.NoneTextFont(),
					connectorPrinter.LeftTextPosition());

			if (data.additional != null)
			{
				foreach (string item in data.additional)
				{
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.PrintDataLine(item));
				}
			}
			return result;
		}

		private byte[] Items()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en Items");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.PrintDataLine(""),
					connectorPrinter.LeftTextPosition());

			if (data.items != null)
			{
				if (data.items.Count > 0 && data.items[0].quantity != null)
				{
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.FontBTextFont(),
							connectorPrinter.BoldTextFont(),
							connectorPrinter.PrintDataLine("CAN".PadRight(4) + "DESCRIPCIÓN".PadRight(30) + "TOTAL".PadRight(2)));
				}
				else
				{
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.FontBTextFont(),
							connectorPrinter.BoldTextFont(),
							connectorPrinter.PrintDataLine("DESCRIPCIÓN".PadRight(36) + "TOTAL".PadRight(2)));
				}

				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.NoneTextFont(),
						connectorPrinter.BoldTextFont(),
						connectorPrinter.PrintDataLine(new string('-', width)));

				foreach (dynamic item in data.items)
				{
					if (item.description is JArray)
					{
						foreach (dynamic descriptionItem in item.description)
						{
							result = connectorPrinter.CombinePrinterParameter(
									result,
									connectorPrinter.NoneTextFont(),
									connectorPrinter.PrintDataLine(descriptionItem.ToString()));
						}

						if (item.totalPrice != null)
						{
							string totalPrice = item.totalPrice.ToString("F2");

							result = connectorPrinter.CombinePrinterParameter(
									result,
									connectorPrinter.RightTextPosition(),
									connectorPrinter.NoneTextFont(),
									connectorPrinter.PrintDataLine(totalPrice));
						}
					}
					else
					{
						if (item.quantity != null && item.description != null)
						{
							string quantity = item.quantity.ToString();
							string description = item.description.ToString();

							string totalPrice = "";
							if (item.totalPrice != null)
							{
								totalPrice = item.totalPrice.ToString("F2");
							}

							string element = "  " + quantity.PadRight(3) + description;

							result = connectorPrinter.CombinePrinterParameter(
									result,
									connectorPrinter.NoneTextFont(),
									connectorPrinter.LeftTextPosition(),
									connectorPrinter.PrintDataLine(connectorPrinter.PadRightText(element, (width - totalPrice.Length), ' ') + totalPrice));
						}

						if (item.commentary != null)
						{
							result = connectorPrinter.CombinePrinterParameter(
									result,
									connectorPrinter.NoneTextFont(),
									connectorPrinter.FontBTextFont(),
									connectorPrinter.PrintDataLine("  => " + item.commentary.ToString()));
						}
					}
				}
				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.LeftTextPosition(),
						connectorPrinter.NoneTextFont(),
						connectorPrinter.BoldTextFont(),
						connectorPrinter.PrintDataLine(new string('-', this.width))
						);
			}
			return result;
		}

		private byte[] Amounts()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en Amounts");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.NoneTextFont(),
					connectorPrinter.RightTextPosition());

			if (data.amounts == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			var jsonString = JsonConvert.SerializeObject(data.amounts);
			var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

			int maxCharacterWidth = CalculateMaxCharacterWidth(dictionary);

			foreach (var kvp in dictionary)
			{
				string field = kvp.Key;
				string value = kvp.Value.ToString();
				int valueLength = (maxCharacterWidth - value.Length);
				string valueFinal = field + " : " + value.PadLeft(maxCharacterWidth, ' ');

				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.PrintDataLine(valueFinal));
			}

			result = connectorPrinter.CombinePrinterParameter(
					result,
					connectorPrinter.LeftTextPosition(),
					connectorPrinter.BoldTextFont(),
					connectorPrinter.PrintDataLine(new string('-', width)));

			return result;
		}

		private byte[] AdditionalFooter()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en AdditionalFooter");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.NoneTextFont(),
					connectorPrinter.LeftTextPosition());

			if (data.additionalFooter == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			foreach (string footerText in data.additionalFooter)
			{
				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.PrintDataLine(footerText));
			}

			result = connectorPrinter.CombinePrinterParameter(
					result,
					connectorPrinter.BoldTextFont(),
					connectorPrinter.PrintDataLine(new string('-', width)));

			return result;
		}

		private byte[] FinalMessage()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en FinalMessage");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.NoneTextFont(),
					connectorPrinter.CenterTextPosition());

			if (data.finalMessage == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			if (data.finalMessage is JArray)
			{
				foreach (string message in data.finalMessage)
				{
					result = connectorPrinter.CombinePrinterParameter(
							result,
							connectorPrinter.PrintDataLine(message));
				}
			}
			else
			{
				result = connectorPrinter.CombinePrinterParameter(
						result,
						connectorPrinter.PrintDataLine(data.finalMessage.ToString()));
			}

			return result;
		}

		private byte[] StringQR()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en StringQR");
			byte[] result = connectorPrinter.CombinePrinterParameter(connectorPrinter.CenterTextPosition());

			if (data.stringQR == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			result = connectorPrinter.CombinePrinterParameter(
					result,
					connectorPrinter.PrintQRCode(data.stringQR.ToString()));

			return result;
		}

		private byte[] ProductionArea()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en ProductionArea");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.CenterTextPosition(),
					connectorPrinter.NoneTextFont());

			if (data.productionArea == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			result = connectorPrinter.CombinePrinterParameter(
					result,
					connectorPrinter.PrintDataLine(data.productionArea.ToString()));

			return result;
		}

		private byte[] TitleExtra()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en TitleExtra");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.CenterTextPosition(),
					connectorPrinter.NoneTextFont());

			if (data.titleExtra == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			result = connectorPrinter.CombinePrinterParameter(
					result,
					connectorPrinter.PrintDataLine(data.titleExtra.title.ToString()),
					connectorPrinter.PrintDataLine(data.titleExtra.subtitle.ToString()));

			return result;
		}

		private byte[] TextBackgroundInverted()
		{
			if (connectorPrinter == null)
				throw new Exception("connectorPrinter = null en TextBackgroundInverted");
			byte[] result = connectorPrinter.CombinePrinterParameter(
					connectorPrinter.CenterTextPosition(),
					connectorPrinter.BoldTextFont(),
					connectorPrinter.NoneTextFont(),
					connectorPrinter.CenterTextPosition());

			if (data.textBackgroundInverted == null)
			{
				result = connectorPrinter.CombinePrinterParameter(connectorPrinter.PrintDataLine(""));
				return result;
			}

			string value = data.textBackgroundInverted;

			result = connectorPrinter.CombinePrinterParameter(
					result,
					connectorPrinter.TextInvertedFont(true),
					connectorPrinter.PrintDataLine(connectorPrinter.PadBoth(value, width, ' ')),
					connectorPrinter.TextInvertedFont(false));
			return result;
		}

		private int CalculateMaxCharacterWidth(Dictionary<string, object> dictionary)
		{
			int maxStringLength = 0;

			foreach (var kvp in dictionary)
			{
				string valueString = kvp.Value.ToString() ?? "";
				int valueLength = valueString.Length;

				if (valueLength > maxStringLength)
				{
					maxStringLength = valueLength;
				}
			}

			return maxStringLength;
		}
	}
}
