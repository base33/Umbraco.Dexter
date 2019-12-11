namespace Dexter.IndexStrategies.Converters
{
    using System;
    using System.Text;
    using System.IO.Compression;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using Umbraco.Core.Logging;

    public class ZipToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(filePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        sb.Append(entry.Name);
                        sb.Append(Environment.NewLine);

                        using (Stream stream = entry.Open())
                        {
                            using (var ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                ms.Position = 0;

                                switch (System.IO.Path.GetExtension(entry.FullName).ToLowerInvariant())
                                {
                                    case ".docx":
                                        this.ReadFromDocx(ms, sb);
                                        break;
                                    case ".xlsx":
                                        this.ReadFromXlsx(ms, sb);
                                        break;
                                    case ".pptx":
                                        this.ReadFromPptx(ms, sb);
                                        break;
                                    case ".pdf":
                                        this.ReadFromPdf(ms, sb);
                                        break;
                                }
                            }
                        }

                        sb.Append(Environment.NewLine);
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    "Error when try extract zip file content",
                    exception);
            }

            return sb.ToString();
        }

        public void ReadFromDocx(MemoryStream ms, StringBuilder sb)
        {
            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(ms, false))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    sb.Append(body.InnerText);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    "Error when try extract zipped docx file content",
                    ex);
            }
        }

        public void ReadFromXlsx(MemoryStream ms, StringBuilder sb)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(ms, false))
                {
                    var converter = new XlsxToTextConverter();
                    converter.GetWorkbookText(document, sb);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    "Error when try extract zipped xlsx file content",
                    ex);
            }
        }

        public void ReadFromPptx(MemoryStream ms, StringBuilder sb)
        {
            try
            {
                using (PresentationDocument document = PresentationDocument.Open(ms, false))
                {
                    var converter = new PptxToTextConverter();
                    converter.ExtractTextFromPresentation(document, sb);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    "Error when try extract zipped pptx file content",
                    ex);
            }
        }

        public void ReadFromPdf(MemoryStream ms, StringBuilder sb)
        {
            try
            {
                var converter = new PdfToTextConverter();
                converter.ReadPdfFile(ms, sb);
            }
            catch (Exception ex)
            {
                LogHelper.Error(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                    "Error when try extract zipped pdf file content",
                    ex);
            }
        }
    }
}
