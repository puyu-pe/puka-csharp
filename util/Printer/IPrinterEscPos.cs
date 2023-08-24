namespace puka.util.printer
{
    public interface IPrinterEscPos
    {
        Task Print(byte[] dataPrintElement);

				Task<bool> IsPrinterOnline();

        byte[] DoubleHeightWeightText();

        byte[] CombinePrinterParameter(params byte[][] dataPrinter);

        byte[] PrintDataLine(string textPrinter);

        byte[] PrintData(string textPrinter);

        byte[] CenterTextPosition();

        byte[] LeftTextPosition();

        byte[] RightTextPosition();
        
        byte[] BoldTextFont();
        
        byte[] FontBTextFont();

        byte[] NoneTextFont();

        byte[] PrintImageData(string imagePath);

        byte[] TextInvertedFont(bool data);

        byte[] PrintQRCode(string dataQR);

        byte[] PrinterCutWidth(int quantity);

        string PadBoth(string text, int width, char PaddingChar);

        string PadRightText(string text, int width, char characterPad);

        byte[] EncodingLatin2();

    }
}
