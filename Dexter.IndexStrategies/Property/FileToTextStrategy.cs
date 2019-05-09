using Dexter.Core.Interfaces;
using Dexter.Core.Models.Config;
using Dexter.Core.Models.IndexStrategy;
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
            var bytes = File.ReadAllBytes(filePath);

            var text = "";

            switch(System.IO.Path.GetExtension(filePath))
            {
                case "pdf":
                    text = ConvertPDFToText(bytes);
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

        private static string ConvertPDFToText(byte[] bytes)
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
