using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QRCoder;
using System.Drawing.Printing;
using DrawingBrushes = System.Drawing.Brushes;
using Image = System.Drawing.Image;

namespace puka.util
{
    public class Printer
    {
        private int width;
        private float positionY;
        private dynamic printer;
        private string type;
        private int times;
        private dynamic data;

        public Printer(dynamic data)
        {
            this.printer = data.printer;
            this.type = data.type;
            this.data = data.data;
            this.times = (data.times == null) ? 1 : data.times;
        }

        public void PrintReceipt()
        {
            int time = int.Parse(this.times.ToString());

            List<PrintDocument> printDocumentList = GeneratePrintDataList(time);

            if (PrinterExists(this.printer.name_system.ToString()))
            {
                foreach (PrintDocument printDocument in printDocumentList)
                {
                    InitPrinterName(printDocument);
                    DefineMarginPage(printDocument);
                    PrintLayout(printDocument);
                }
            }
            else
            {
                MessageBox.Show("No se encontró una impresora predeterminada.");
            }
        }
        private List<PrintDocument> GeneratePrintDataList(int times)
        {
            List<PrintDocument> printDocumentList = new List<PrintDocument>();

            for (int i = 0; i < times; i++)
            {
                PrintDocument printDocument = new PrintDocument();
                printDocumentList.Add(printDocument);
            }

            return printDocumentList;
        }

        private void InitPrinterName(PrintDocument printDocument)
        {
            string printerName = this.printer.name_system.ToString();
            printDocument.PrinterSettings.PrinterName = printerName;
        }

        private bool PrinterExists(string printerName)
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                if (printer == printerName)
                {
                    return true;
                }
            }
            return false;
        }

        private void DefineMarginPage(PrintDocument printDocument)
        {
            float leftMargin = 0.1f;   // Margen izquierdo de 1 pulgada
            float rightMargin = 0.1f;  // Margen derecho de 1 pulgada
            float topMargin = 0.1f;    // Margen superior de 1.5 pulgadas
            float bottomMargin = 0.5f; // Margen inferior de 1 pulgada

            int leftMarginInPixels = (int)(leftMargin * 100);
            int rightMarginInPixels = (int)(rightMargin * 100);
            int topMarginInPixels = (int)(topMargin * 100);
            int bottomMarginInPixels = (int)(bottomMargin * 100);

            printDocument.DefaultPageSettings.Margins = new Margins(leftMarginInPixels, rightMarginInPixels, topMarginInPixels, bottomMarginInPixels);
            this.width = printDocument.DefaultPageSettings.PaperSize.Width;
        }

        private void PrintLayout(PrintDocument printDocument)
        {
            try
            {
                using (printDocument)
                {
                    Header(printDocument);

                    switch (this.type)
                    {
                        case "invoice":
                            BusinessAdditional(printDocument);
                            DocumentLegal(printDocument);
                            Customer(printDocument);
                            Additional(printDocument);
                            Items(printDocument);
                            Amounts(printDocument);
                            AdditionalFooter(printDocument);
                            FinalMessage(printDocument);
                            StringQR(printDocument);
                            break;

                        case "note":
                            DocumentLegal(printDocument);
                            Customer(printDocument);
                            Additional(printDocument);
                            break;

                        case "command":
                            ProductionArea(printDocument);
                            TextBackgroundInverted(printDocument);
                            DocumentLegal(printDocument);
                            Additional(printDocument);
                            Items(printDocument);
                            break;

                        case "precount":
                            DocumentLegal(printDocument);
                            Additional(printDocument);
                            Items(printDocument);
                            Amounts(printDocument);
                            break;

                        case "extra":
                            TitleExtra(printDocument);
                            Additional(printDocument);
                            Items(printDocument);
                            Amounts(printDocument);
                            break;

                        default:
                            MessageBox.Show("No se pudo conectar con la tiketera");
                            throw new Exception("No se pudo conectar con la tiketera");

                    }

                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void Header(PrintDocument printDocument)
        {
            StringFormat stringFormat = new StringFormat();

            stringFormat = TextFormated();
            stringFormat.Alignment = StringAlignment.Center;

            if (this.data.business.comercialDescription != null)
            {
                if (this.data.business.comercialDescription.type == "text")
                {
                    printDocument.PrintPage += (sender, e) =>
                    {
                        Font font = new Font("Arial", 14, FontStyle.Bold);
                        RectangleF textRect = TextCenterValue((this.data.business.comercialDescription.value.ToString()).ToUpper(), e, font, stringFormat);
                        e.Graphics.DrawString((this.data.business.comercialDescription.value.ToString()).ToUpper(), font, DrawingBrushes.Black, textRect, stringFormat);
                        this.positionY += textRect.Height;
                    };
                }
                else if (this.data.business.comercialDescription.type == "img")
                {
                    string appDirectory = Path.Combine(Directory.GetCurrentDirectory(), "img");

                    string imagePath = Path.Combine(appDirectory, "logo.png");

                    if (!File.Exists(imagePath))
                    {
                        MessageBox.Show(imagePath);
                        throw new Exception("No se encontró el logo");
                    }

                    printDocument.PrintPage += (sender, e) =>
                    {
                        Image logoImage = Image.FromFile(imagePath);
                        int centerX = (e.MarginBounds.Left + e.MarginBounds.Right) / 2;
                        int imageX = centerX - (logoImage.Width / 2);

                        Rectangle logoRect = new Rectangle(imageX, e.MarginBounds.Top, logoImage.Width, logoImage.Height);
                        e.Graphics.DrawImage(logoImage, logoRect, new Rectangle(0, 0, logoImage.Width, logoImage.Height), GraphicsUnit.Pixel);
                        this.positionY += logoRect.Height * 1.1f;
                    };
                }
            }
            else
            {
                printDocument.PrintPage += (sender, e) =>
                {
                    this.positionY = e.MarginBounds.Y;
                };
            }

            if (this.data.business.description != null)
            {
                printDocument.PrintPage += (sender, e) =>
                {
                    Font font = new Font("Arial", 10, FontStyle.Regular);
                    RectangleF textRectDescription = TextCenterValue(this.data.business.description.ToString(), e, font, stringFormat);
                    e.Graphics.DrawString(this.data.business.description.ToString(), font, DrawingBrushes.Black, textRectDescription, stringFormat);
                    this.positionY += textRectDescription.Height * 1.5f;
                };
            }
        }

        private void BusinessAdditional(PrintDocument printDocument)
        {
            if (this.data.business.additional == null)
                return;

            StringFormat stringFormat = new StringFormat();

            stringFormat = TextFormated();

            printDocument.PrintPage += (sender, e) =>
            {
                Graphics graphics = e.Graphics;
                Font font = new Font("Arial", 10, FontStyle.Regular);
                stringFormat.Alignment = StringAlignment.Center;

                foreach (string additional in this.data.business.additional)
                {
                    RectangleF textRectAdditional = TextCenterValue(additional, e, font, stringFormat);
                    e.Graphics.DrawString(additional, font, DrawingBrushes.Black, textRectAdditional, stringFormat);
                    this.positionY += textRectAdditional.Height;
                }
            };
        }

        private void DocumentLegal(PrintDocument printDocument)
        {
            StringFormat stringFormat = new StringFormat();

            stringFormat = TextFormated();

            Font font = new Font("Arial", 8, FontStyle.Bold);

            switch (this.type)
            {
                case "invoice":
                case "note":
                case "command":
                    stringFormat.Alignment = StringAlignment.Center;

                    if (this.data.document != null)
                    {
                        printDocument.PrintPage += (sender, e) =>
                        {
                            string text = this.data.document.description.ToString() + "  " + this.data.document.indentifier.ToString();
                            RectangleF textRectDocument = TextCenterValue(text, e, font, stringFormat);
                            e.Graphics.DrawString(text, font, DrawingBrushes.Black, textRectDocument, stringFormat);
                            this.positionY += textRectDocument.Height;
                        };
                    }
                    else
                    {
                        printDocument.PrintPage += (sender, e) =>
                        {
                            string text = this.data.document.ToString() + "  " + this.data.documentId.ToString();
                            RectangleF textRectDocumentLegal = TextCenterValue(text, e, font, stringFormat);
                            e.Graphics.DrawString(text, font, DrawingBrushes.Black, textRectDocumentLegal, stringFormat);
                            this.positionY += textRectDocumentLegal.Height;
                        };
                    }
                    break;

                case "precount":
                    printDocument.PrintPage += (sender, e) =>
                    {
                        RectangleF textRectDescription = TextLeftValue(this.data.document.description.ToString(), e, font, stringFormat);
                        e.Graphics.DrawString(this.data.document.description.ToString(), font, DrawingBrushes.Black, textRectDescription, stringFormat);
                        this.positionY += textRectDescription.Height;
                    };
                    break;
            }
        }

        private void Customer(PrintDocument printDocument)
        {
            Font regularFont = new Font("Arial", 8, FontStyle.Regular);
            StringFormat stringFormat = new StringFormat();

            stringFormat = TextFormated();

            printDocument.PrintPage += (sender, e) =>
            {
                float lineHeight = regularFont.GetHeight(e.Graphics);

                if (this.data.customer != null)
                {
                    foreach (string row in this.data.customer)
                    {
                        RectangleF textRectCustomer = TextLeftValue(row, e, regularFont, stringFormat);
                        e.Graphics.DrawString(row, regularFont, DrawingBrushes.Black, textRectCustomer, stringFormat);
                        this.positionY += textRectCustomer.Height;
                    }
                }
                /*else
                {
                    string defaultCustomer = "--".PadRight(this.width);
                    e.Graphics.DrawString(defaultCustomer, new Font("Arial", 10, FontStyle.Regular), DrawingBrushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
                    this.positionY += lineHeight;
                }*/
            };
        }

        private void Additional(PrintDocument printDocument)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat = TextFormated();

            printDocument.PrintPage += (sender, e) =>
            {
                Font font = new Font("Arial", 8, FontStyle.Regular);
                float lineHeight = font.GetHeight(e.Graphics);

                if (this.data.additional != null)
                {
                    foreach (string item in this.data.additional)
                    {
                        RectangleF textRectAdditional = TextLeftValue(item, e, font, stringFormat);
                        e.Graphics.DrawString(item, font, DrawingBrushes.Black, textRectAdditional, stringFormat);
                        this.positionY += textRectAdditional.Height;
                    }
                }
                this.positionY += lineHeight;
            };
        }

        private void Items(PrintDocument printDocument)
        {
            printDocument.PrintPage += (sender, e) =>
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat = TextFormated();

                Font emphasisFont = new Font("Arial", 8, FontStyle.Bold);
                Font regularFont = new Font("Arial", 8, FontStyle.Regular);
                float lineHeight = regularFont.GetHeight(e.Graphics);

                if (this.data.items != null)
                {
                    string totalText = "TOTAL".PadRight(7);

                    float totalXPos = PositionXRight(totalText, regularFont, e);

                    if (this.data.items.Count > 0 && this.data.items[0].quantity != null)
                    {
                        e.Graphics.DrawString("CAN".PadLeft(4) + " DESCRIPCIÓN", emphasisFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                        //e.Graphics.DrawString(totalText, emphasisFont, DrawingBrushes.Black, totalXPos, this.positionY);
                    }
                    else
                    {
                        e.Graphics.DrawString("DESCRIPCIÓN", emphasisFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                        e.Graphics.DrawString(totalText, emphasisFont, DrawingBrushes.Black, totalXPos, this.positionY);
                    }

                    this.positionY += lineHeight;
                    e.Graphics.DrawString(new string('-', e.MarginBounds.Width), regularFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                    this.positionY += lineHeight;

                    foreach (dynamic item in this.data.items)
                    {
                        if (item.description is JArray)
                        {
                            foreach (dynamic descriptionItem in item.description)
                            {
                                string description = descriptionItem.ToString().PadRight(35);
                                RectangleF textRectItem = TextLeftValue(description, e, regularFont, stringFormat);
                                e.Graphics.DrawString(description, regularFont, DrawingBrushes.Black, textRectItem, stringFormat);
                                this.positionY += textRectItem.Height;
                            }

                            if (item.totalPrice != null)
                            {
                                string totalPrice = item.totalPrice.ToString("F2").PadRight(7);
                                float totalPositionX = PositionXRight(totalPrice, regularFont, e);
                                e.Graphics.DrawString(totalPrice, regularFont, DrawingBrushes.Black, totalPositionX, this.positionY, stringFormat);
                                this.positionY += lineHeight;
                            }
                        }
                        else
                        {
                            if (item.quantity != null && item.description != null)
                            {
                                string quantity = item.quantity.ToString().PadLeft(7);
                                string description = ("  " + item.description.ToString());

                                string totalPrice = "";
                                if (item.totalPrice != null)
                                {
                                    totalPrice = item.totalPrice.ToString("F2").PadLeft(7);
                                }

                                float totalPositionX = PositionXRight(totalPrice, regularFont, e);

                                e.Graphics.DrawString(quantity + description, regularFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                                e.Graphics.DrawString(totalPrice, regularFont, DrawingBrushes.Black, totalPositionX, this.positionY, stringFormat);
                                this.positionY += lineHeight;
                            }

                            if (item.commentary != null)
                            {
                                e.Graphics.DrawString(" " + "=> " + item.commentary.ToString().PadRight(35), regularFont, DrawingBrushes.Black, e.MarginBounds.Left + 7, this.positionY);
                                this.positionY += lineHeight;
                            }
                        }
                    }

                    e.Graphics.DrawString(new string('-', e.MarginBounds.Width), regularFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                    this.positionY += lineHeight;
                }
            };
        }

        private int CalculateMaxCharacterWidth(Dictionary<string, object> dictionary)
        {
            int maxStringLength = 0;

            foreach (var kvp in dictionary)
            {
                string valueString = kvp.Key.ToString();
                int valueLength = valueString.Length;

                if (valueLength > maxStringLength)
                {
                    maxStringLength = valueLength;
                }
            }

            return maxStringLength;
        }

        private void Amounts(PrintDocument printDocument)
        {
            if (this.data.amounts == null)
                return;

            printDocument.PrintPage += (sender, e) =>
            {
                Font regularFont = new Font("Arial", 8, FontStyle.Regular);
                float lineHeight = regularFont.GetHeight(e.Graphics);

                var jsonString = JsonConvert.SerializeObject(this.data.amounts);
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                int maxCharacterWidth = CalculateMaxCharacterWidth(dictionary);

                foreach (var kvp in dictionary)
                {
                    string field = kvp.Key;
                    string value = kvp.Value.ToString();
                    int valueLength = (maxCharacterWidth - field.Length) * 2;

                    string valueFinal = field + " : " + value.PadLeft(valueLength);

                    float totalPositionX = PositionXRight(valueFinal, regularFont, e);
                    e.Graphics.DrawString(valueFinal, regularFont, DrawingBrushes.Black, totalPositionX, this.positionY);

                    this.positionY += lineHeight;
                }

                e.Graphics.DrawString(new string('-', e.MarginBounds.Width), regularFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                this.positionY += lineHeight;
            };
        }

        private void AdditionalFooter(PrintDocument printDocument)
        {
            if (data.additionalFooter == null)
                return;

            StringFormat stringFormat = new StringFormat();
            stringFormat = TextFormated();
            Font regularFont = new Font("Arial", 8, FontStyle.Regular);

            printDocument.PrintPage += (sender, e) =>
            {

                float lineHeight = regularFont.GetHeight(e.Graphics);

                foreach (string footerText in data.additionalFooter)
                {
                    RectangleF textRectFooter = TextLeftValue(footerText, e, regularFont, stringFormat);
                    e.Graphics.DrawString(footerText, regularFont, DrawingBrushes.Black, textRectFooter, stringFormat);
                    this.positionY += textRectFooter.Height;
                }

                e.Graphics.DrawString(new string('-', width), regularFont, DrawingBrushes.Black, e.MarginBounds.Left, this.positionY);
                this.positionY += lineHeight;
            };
        }

        private void FinalMessage(PrintDocument printDocument)
        {
            if (this.data.finalMessage == null)
                return;

            StringFormat stringFormat = new StringFormat();
            stringFormat = TextFormated();
            Font regularFont = new Font("Arial", 8, FontStyle.Regular);

            printDocument.PrintPage += (sender, e) =>
            {
                if (this.data.finalMessage is JArray)
                {
                    foreach (string message in this.data.finalMessage)
                    {
                        RectangleF textRectMessage = TextLeftValue(message, e, regularFont, stringFormat);
                        e.Graphics.DrawString(message, regularFont, DrawingBrushes.Black, textRectMessage, stringFormat);
                        this.positionY += textRectMessage.Height;
                    }
                }
                else
                {
                    RectangleF textRectMessage = TextCenterValue(this.data.finalMessage.ToString(), e, regularFont, stringFormat);
                    e.Graphics.DrawString(this.data.finalMessage.ToString(), regularFont, DrawingBrushes.Black, textRectMessage, stringFormat);
                    this.positionY += textRectMessage.Height;
                }
            };
        }

        private void StringQR(PrintDocument printDocument)
        {
            if (this.data.stringQR == null)
                return;

            printDocument.PrintPage += (sender, e) =>
            {
                Font regularFont = new Font("Arial", 10, FontStyle.Regular);

                float lineHeight = regularFont.GetHeight(e.Graphics);
                float qrWidth = 100;
                float qrXPos = e.MarginBounds.Left + (e.MarginBounds.Width - qrWidth) / 2;


                if (printer.name_system == "127.0.0.1" && printer.type == "ethernet")
                {
                    e.Graphics.DrawString(this.data.stringQR.ToString(), regularFont, DrawingBrushes.Black, qrXPos, this.positionY);
                    this.positionY += lineHeight;
                }
                else
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(this.data.stringQR.ToString(), QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);

                    // Tamaño actual dividido por 2
                    Bitmap qrCodeImage = qrCode.GetGraphic(2);

                    e.Graphics.DrawImage(qrCodeImage, qrXPos, this.positionY);
                    this.positionY += qrCodeImage.Height + lineHeight;
                }
            };
        }

        private void ProductionArea(PrintDocument printDocument)
        {
            string text = $" {this.data.productionArea} ";
            Font font = new Font("Arial", 10, FontStyle.Regular);

            printDocument.PrintPage += (sender, e) =>
            {
                Graphics graphics = e.Graphics;
                SizeF textSize = graphics.MeasureString(text, font);
                float x = (this.width - textSize.Width) / 2;

                graphics.DrawString(text, font, DrawingBrushes.Black, x, this.positionY);

                this.positionY += font.GetHeight(e.Graphics) * 1.5f;
            };
        }

        private void TextBackgroundInverted(PrintDocument printDocument)
        {
            if (this.data.textBackgroundInverted == null)
                return;

            Font font = new Font("Arial", 10, FontStyle.Regular);
            string text = $" {this.data.textBackgroundInverted} ";

            printDocument.PrintPage += (sender, e) =>
            {
                Graphics graphics = e.Graphics;

                // Obtener el tamaño del texto
                SizeF textSize = graphics.MeasureString(text, font);

                // Calcular las coordenadas x e y para centrar el texto horizontalmente en el rectángulo
                int pageWidth = e.PageBounds.Width;
                float x = (pageWidth - textSize.Width) / 2;
                float y = this.positionY;

                // Ancho del rectángulo que ocupa toda la anchura de la página (márgenes izquierdo y derecho)
                float rectangleWidth = e.MarginBounds.Right - e.MarginBounds.Left;

                // Color del fondo invertido (negro)
                Color invertedBackColor = Color.Black;

                // Dibujar el fondo invertido (rectángulo negro)
                graphics.FillRectangle(new SolidBrush(invertedBackColor), e.MarginBounds.Left, y, rectangleWidth, textSize.Height);

                // Cambiar el color del texto a blanco
                SolidBrush textBrush = new SolidBrush(Color.White);

                // Dibujar el texto en el rectángulo con color blanco
                graphics.DrawString(text, font, textBrush, x, y);

                this.positionY += (int)textSize.Height * 1.5f;
            };
        }

        private void TitleExtra(PrintDocument printDocument)
        {
            Font titleFont = new Font("Arial", 14, FontStyle.Regular); // Ajusta la fuente y el tamaño según tus necesidades
            Font subtitleFont = new Font("Arial", 10, FontStyle.Regular); // Ajusta la fuente y el tamaño según tus necesidades

            string title = this.data.titleExtra.title;
            string subtitle = this.data.titleExtra.subtitle;

            printDocument.PrintPage += (sender, e) =>
            {
                Graphics graphics = e.Graphics;

                float lineHeight = titleFont.GetHeight(e.Graphics);

                float pageWidth = e.PageSettings.PaperSize.Width;
                float pageHeight = e.PageSettings.PaperSize.Height;
                float startY = (pageHeight - titleFont.Height - subtitleFont.Height) / 2;

                graphics.DrawString(title, titleFont, DrawingBrushes.Black, new PointF(pageWidth / 2, this.positionY), new StringFormat() { Alignment = StringAlignment.Center });
                this.positionY += lineHeight;

                lineHeight = subtitleFont.GetHeight(e.Graphics);

                graphics.DrawString(subtitle, subtitleFont, DrawingBrushes.Black, new PointF(pageWidth / 2, this.positionY), new StringFormat() { Alignment = StringAlignment.Center });
                this.positionY += (lineHeight * 1.5f);
            };

        }

        private StringFormat TextFormated()
        {
            StringFormat stringFormat = new StringFormat();

            stringFormat.Trimming = StringTrimming.Word;
            stringFormat.FormatFlags = StringFormatFlags.LineLimit;

            return stringFormat;
        }

        private float PositionXRight(string text, Font font, PrintPageEventArgs e)
        {
            SizeF totalTextSize = e.Graphics.MeasureString(text, font);
            float totalXPos = e.MarginBounds.Right - totalTextSize.Width;

            return totalXPos;
        }

        private RectangleF TextCenterValue(string text, PrintPageEventArgs e, Font font, StringFormat stringFormat)
        {
            int pageWidth = e.PageBounds.Width;
            SizeF textSize = e.Graphics.MeasureString(text, font, pageWidth, stringFormat);
            float x = (pageWidth - textSize.Width) / 2;
            RectangleF textRect = new RectangleF(x, this.positionY, textSize.Width, textSize.Height);

            return textRect;
        }

        private RectangleF TextLeftValue(string text, PrintPageEventArgs e, Font font, StringFormat stringFormat)
        {
            Graphics graphics = e.Graphics;
            int marginLeft = e.MarginBounds.Left;
            int pageWidth = e.PageBounds.Width;

            SizeF textSize = graphics.MeasureString(text, font, pageWidth, stringFormat);
            RectangleF textRect = new RectangleF(marginLeft, this.positionY, textSize.Width, textSize.Height);

            return textRect;
        }
    }
}
