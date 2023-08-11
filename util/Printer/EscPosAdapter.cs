using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using printer_aplication_desktop.utils;
using System.Text;
using System.Text.RegularExpressions;

namespace printer_aplication_desktop.components
{
    public class EscPosAdapter : IPrinterEscPos
    {
        private ListTypeConexion typeConexion;
        private EPSON elementDataPrinter;
        private object printer;
        private object hostname;
        private object port;
        private Dictionary<char, char> characterMap;

        public EscPosAdapter(object hostname, object port, ListTypeConexion typeConexion)
        {
            elementDataPrinter = new EPSON();
            this.hostname = hostname;
            this.port = port;
            this.typeConexion = typeConexion;
            Conexion();
            SpecialCharacterToLetterReplacer();
        }

        private void SpecialCharacterToLetterReplacer()
        {
            characterMap = new Dictionary<char, char>
            {
                { 'á', 'a' }, { 'Á', 'A' },
                { 'à', 'a' }, { 'À', 'A' },
                { 'â', 'a' }, { 'Â', 'A' },
                { 'ã', 'a' }, { 'Ã', 'A' },
                { 'ä', 'a' }, { 'Ä', 'A' },
                { 'é', 'e' }, { 'É', 'E' },
                { 'è', 'e' }, { 'È', 'E' },
                { 'ê', 'e' }, { 'Ê', 'E' },
                { 'ë', 'e' }, { 'Ë', 'E' },
                { 'í', 'i' }, { 'Í', 'I' },
                { 'ì', 'i' }, { 'Ì', 'I' },                
                { 'î', 'i' }, { 'Î', 'I' },
                { 'ï', 'i' }, { 'Ï', 'I' },
                { 'ó', 'o' }, { 'Ó', 'O' },
                { 'ò', 'o' }, { 'Ò', 'O' },
                { 'ô', 'o' }, { 'Ô', 'O' },
                { 'õ', 'o' }, { 'Õ', 'O' },
                { 'ö', 'o' }, { 'Ö', 'O' },
                { 'ú', 'u' }, { 'Ú', 'U' },
                { 'ù', 'u' }, { 'Ù', 'U' },
                { 'û', 'u' }, { 'Û', 'U' },
                { 'ü', 'u' }, { 'Ü', 'U' },
                { 'ñ', 'n' }, { 'Ñ', 'N' },
            };
        }

        private string ReplaceSpecialCharactersWithLetters(string input)
        {
            string replacedString = Regex.Replace(input, "[áàâãäéèêëíìîïóòôõöúùûüñÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÑ]", m => characterMap[m.Value[0]].ToString(), RegexOptions.IgnoreCase);

            return replacedString;
        }

        private void Conexion() 
        {
            switch (typeConexion)
            {
                case ListTypeConexion.ImmediateNetworkPrinter:
                    printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings()
                    {
                        ConnectionString = $"{hostname}:{port}",
                        PrinterName = "PrinterConnector"
                    });
                    break;
                case ListTypeConexion.SerialPrinter:
                    int Bound = Convert.ToInt32(port.ToString());
                    printer = new SerialPrinter(hostname.ToString(), Bound);
                    break;
                case ListTypeConexion.FilePrinter:
                    printer = new FilePrinter(hostname.ToString());
                    break;
                case ListTypeConexion.SambaPrinter:
                    printer = new SambaPrinter(hostname.ToString(), port.ToString());
                    break;
                default:
                    throw new ArgumentException("Tipo de impresora no válido.");
            }
        }

        public void Print(byte[] dataPrintElement)
        {
            if (printer is ImmediateNetworkPrinter printerNetWork) 
            {
                printerNetWork.WriteAsync(CombinePrinterParameter(dataPrintElement));
            }

            if (printer is SerialPrinter printerSerial)
            {
                printerSerial.Write(CombinePrinterParameter(dataPrintElement));
                printerSerial.Dispose();
            }

            if (printer is FilePrinter printerFile)
            {
                printerFile.Write(CombinePrinterParameter(dataPrintElement));
                printerFile.Dispose();
            }

            if (printer is SambaPrinter printerSamba)
            {
                printerSamba.Write(CombinePrinterParameter(dataPrintElement));
                printerSamba.Dispose();
            }
        }

        public byte[] CombinePrinterParameter(params byte[][] dataPrinter)
        {
            byte[] builder = null;

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
            byte[] elementDouble = CombinePrinterParameter(elementDataPrinter.SetStyles(PrintStyle.DoubleHeight | PrintStyle.DoubleWidth));

            return elementDouble;
        }

        public byte[] PrintDataLine(string textPrinter)
        {
            string textReplaced = ReplaceSpecialCharactersWithLetters(textPrinter);

            byte[] elementText = CombinePrinterParameter(elementDataPrinter.PrintLine(textReplaced));

            return elementText;
        }

        public byte[] PrintData(string textPrinter)
        {
            string textReplaced = ReplaceSpecialCharactersWithLetters(textPrinter);

            byte[] elementText = CombinePrinterParameter(elementDataPrinter.Print(textReplaced));

            return elementText;
        }

        public byte[] CenterTextPosition()
        {
            byte[] elementCenter = CombinePrinterParameter(elementDataPrinter.CenterAlign());

            return elementCenter;
        }

        public byte[] LeftTextPosition()
        {
            byte[] elementLeft = CombinePrinterParameter(elementDataPrinter.LeftAlign());

            return elementLeft;
        }

        public byte[] RightTextPosition()
        {
            byte[] elementRight = CombinePrinterParameter(elementDataPrinter.RightAlign());

            return elementRight;
        }

        public byte[] BoldTextFont()
        {
            byte[] fontBold = CombinePrinterParameter(elementDataPrinter.SetStyles(PrintStyle.Bold));

            return fontBold;
        }

        public byte[] FontBTextFont()
        {
            byte[] fontB = CombinePrinterParameter(elementDataPrinter.SetStyles(PrintStyle.FontB));

            return fontB;
        }

        public byte[] NoneTextFont()
        {
            byte[] fontNone = CombinePrinterParameter(elementDataPrinter.SetStyles(PrintStyle.None));

            return fontNone;
        }

        public byte[] PrintImageData(string imagePath)
        {
            byte[] dataImagePrinter = CombinePrinterParameter(
                        elementDataPrinter.PrintImage(File.ReadAllBytes(imagePath), true),
                        PrintDataLine(""));

            return dataImagePrinter;
        }

        public byte[] TextInvertedFont(bool modeText)
        {
            byte[] textInvertedPrinter = CombinePrinterParameter(elementDataPrinter.ReverseMode(modeText));

            return textInvertedPrinter;
        }

        public byte[] PrintQRCode(string dataQR)
        {
            byte[] dataQRPrinter = CombinePrinterParameter(elementDataPrinter.PrintQRCode(dataQR));

            return dataQRPrinter;
        }

        public byte[] PrinterCutWidth(int quantity)
        {
            byte[] cutPrinter = CombinePrinterParameter(elementDataPrinter.FullCutAfterFeed(quantity));

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
