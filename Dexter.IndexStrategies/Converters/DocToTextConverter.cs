namespace Dexter.IndexStrategies.Converters
{
    using System;
    using System.Text;

    public class DocToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }
    }
}
