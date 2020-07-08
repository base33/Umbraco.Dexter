namespace Dexter.IndexStrategies.Converters
{
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;
    using System;
    using System.IO;
    using System.Text;

    public class PdfToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                this.ReadPdfFile(filePath, sb);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }

        private void ReadPdfFile(string filePath, StringBuilder sb)
        {
            var bytes = File.ReadAllBytes(filePath);
            var reader = new PdfReader(bytes);
            var numberOfPages = reader.NumberOfPages;

            for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
            {
                sb.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
            }
        }

        public void ReadPdfFile(MemoryStream ms, StringBuilder sb)
        {
            var reader = new PdfReader(ms);
            var numberOfPages = reader.NumberOfPages;

            for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
            {
                sb.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
            }
        }
    }
}
