namespace Dexter.IndexStrategies.Converters
{
    using DocumentFormat.OpenXml.Packaging;
    using System;
    using System.Text;

    public class DocxToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(filePath, false))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    sb.Append(body.InnerText);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }
    }
}
