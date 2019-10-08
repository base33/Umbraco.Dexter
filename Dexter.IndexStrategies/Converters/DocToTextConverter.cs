namespace Dexter.IndexStrategies.Converters
{
    using Spire.Doc;
    using System;
    using System.Text;

    public class DocToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                Document document = new Document();
                document.LoadFromFile(filePath);
                sb.Append(document.GetText());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }
    }
}
