namespace Dexter.IndexStrategies.Property
{
    using Dexter.Core.Interfaces;
    using Dexter.Core.Models.IndexStrategy;
    using Dexter.IndexStrategies.Converters;
    using System.Configuration;
    using System.Linq;

    public class FileToTextStrategy : IPropertyIndexStrategy
    {
        protected string[] IGNORE = new[] { "the", "of", "a", "but", "there", "where", "\n", "for", "and" };

        public void Execute(IndexFieldEvent e)
        {
            var umbracoFileName = e.UmbracoProperty.Value != null ? e.UmbracoProperty.Value.ToString() : string.Empty;
            var docPath = ConfigurationManager.AppSettings["Dexter:DocumentPath"];
            var filePath = string.IsNullOrWhiteSpace(docPath) 
                ? umbracoFileName 
                : umbracoFileName.Replace("~/media", ConfigurationManager.AppSettings["Dexter:DocumentPath"])
                        .Replace("/media", ConfigurationManager.AppSettings["Dexter:DocumentPath"]);

            var text = string.Empty;

            switch(System.IO.Path.GetExtension(filePath))
            {
                case ".pdf":
                    text = new PdfToTextConverter().Convert(filePath);
                    break;
                case ".doc":
                    text = new DocToTextConverter().Convert(filePath);
                    break;
                case ".xls":
                    text = new XlsToTextConverter().Convert(filePath);
                    break;
                case ".docx":
                    text = new DocxToTextConverter().Convert(filePath);
                    break;
                case ".xlsx":
                    text = new XlsxToTextConverter().Convert(filePath);
                    break;
                case ".pptx":
                    text = new PptxToTextConverter().Convert(filePath);
                    break;
                case ".ppt":
                    text = System.IO.Path.GetFileName(filePath);
                    break;
                case ".zip":
                    text = new ZipToTextConverter().Convert(filePath);
                    break;
            }

            e.Value = string.Join(" ", text.Split(new[] { ' ' }).Except(IGNORE));
        }
    }
}
