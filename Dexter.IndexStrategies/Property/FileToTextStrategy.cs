namespace Dexter.IndexStrategies.Property
{
    using Dexter.Core.Interfaces;
    using Dexter.Core.Models.Config;
    using Dexter.Core.Models.IndexStrategy;
    using Dexter.IndexStrategies.Converters;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Umbraco.Core;

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
                    text = new XlsToTextConverter().Convert(filePath);
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
    }
}
