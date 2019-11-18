namespace Dexter.IndexStrategies.Converters
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using DocumentFormat.OpenXml.Presentation;
    using A = DocumentFormat.OpenXml.Drawing;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml;
    using System.Text;

    public class PptxToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                using (PresentationDocument document = PresentationDocument.Open(filePath, false))
                {
                    if (document == null)
                    {
                        throw new ArgumentNullException("presentationDocument doesn't exist or unreadable.");
                    }

                    this.ExtractTextFromPresentation(document, sb);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }

        public void ExtractTextFromPresentation(PresentationDocument document, StringBuilder sb)
        {
            PresentationPart pPart = document.PresentationPart;

            int slidesCount = 0;
            if (pPart != null)
            {
                slidesCount = pPart.SlideParts.Count();
            }

            if (slidesCount < 1)
            {
                return;
            }

            OpenXmlElementList slideIds = pPart.Presentation.SlideIdList.ChildElements;

            for (var i = 0; i < slidesCount; i++)
            {
                string relId = (slideIds[i] as SlideId).RelationshipId;

                SlidePart slide = (SlidePart)pPart.GetPartById(relId);

                IEnumerable<A.Text> texts = slide.Slide.Descendants<A.Text>();
                foreach (A.Text text in texts)
                {
                    sb.Append(text.Text);
                }
            }
        }
    }
}
