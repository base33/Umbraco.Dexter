namespace Dexter.IndexStrategies.Converters
{
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class XlsxToTextConverter : IPropertyConverter<string, string>
    {
        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                this.GetWorkbookText(filePath, sb);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }

        private void GetWorkbookText(string filePath, StringBuilder sb)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                var docSheets = wbPart.Workbook.Descendants<Sheet>();

                foreach(var currentSheet in docSheets)
                {
                    WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(currentSheet.Id));

                    var cells = wsPart.Worksheet.Descendants<Cell>();

                    this.GetCellsText(wbPart, cells, sb);
                }
            }
        }

        private void GetCellsText(WorkbookPart wbPart, IEnumerable<Cell> sheetCells, StringBuilder sb)
        {
            foreach(var currentCell in sheetCells)
            {
                string value = null;

                if (currentCell.InnerText.Length > 0)
                {
                    value = currentCell.InnerText;

                    if (currentCell.DataType != null)
                    {
                        switch (currentCell.DataType.Value)
                        {
                            case CellValues.SharedString:
                                
                                var stringTable =
                                    wbPart.GetPartsOfType<SharedStringTablePart>()
                                    .FirstOrDefault();

                                if (stringTable != null)
                                {
                                    value = stringTable.SharedStringTable
                                        .ElementAt(int.Parse(value)).InnerText;
                                }

                                break;

                            case CellValues.Boolean:
                                switch (value)
                                {
                                    case "0":
                                        value = "FALSE";
                                        break;
                                    default:
                                        value = "TRUE";
                                        break;
                                }
                                break;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(value))
                {
                    sb.Append(value);
                    sb.Append("; ");
                }
            }
        }
    }
}
