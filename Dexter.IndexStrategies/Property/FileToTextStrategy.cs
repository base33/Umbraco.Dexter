using Dexter.Core.Interfaces;
using Dexter.Core.Models.Config;
using Dexter.Core.Models.IndexStrategy;
using Dexter.IndexStrategies.Converters;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;

namespace Dexter.IndexStrategies.Property
{
    public class FileToTextStrategy : IPropertyIndexStrategy
    {
        protected string[] IGNORE = new[] { "the", "of", "a", "but", "there", "where", "\n", "for", "and" };

        public void Execute(IndexFieldEvent e)
        {
            var filePath = HttpContext.Current.Server.MapPath(e.UmbracoProperty.Value.ToString());

            var text = string.Empty;

            switch(System.IO.Path.GetExtension(filePath))
            {
                case ".pdf":
                    text = new PdfToTextConverter().Convert(filePath);
                    break;
                case ".doc":
                case ".xls":
                    break;
                case ".docx":
                    text = new DocxToTextConverter().Convert(filePath);
                    break;
                case ".xlsx":
                    text = new XlsxToTextConverter().Convert(filePath);
                    break;
            }

            e.Value = string.Join(" ", text.Split(new[] { ' ' }).Except(IGNORE));
        }

        private static string ConvertDocToText(byte[] bytes)
        {
            var sb = new StringBuilder();

            try
            {
                var reader = new PdfReader(bytes);
                var numberOfPages = reader.NumberOfPages;

                for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                {
                    sb.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
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
