namespace Dexter.IndexStrategies.Converters
{
    using DocumentFormat.OpenXml.Packaging;
    using System;
    using System.Text;
    using A = DocumentFormat.OpenXml.Wordprocessing;

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

                    if (!body.HasChildren) { return string.Empty; }

                    foreach (var child in body.ChildElements)
                    {
                        if (child is A.Table)
                        {
                            var tableContent = child.Descendants<A.Text>();
                            foreach (var cell in tableContent)
                            {
                                sb.Append(cell.InnerText);
                                sb.Append(" ");
                            }
                        }
                        else
                        {
                            sb.Append(child.InnerText);
                        }
                        sb.Append(" ");
                    }
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
