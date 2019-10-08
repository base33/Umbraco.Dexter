namespace Dexter.IndexStrategies.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Linq;
    using System.Text;

    public class XlsToTextConverter : IPropertyConverter<string, string>
    {
        private readonly string xlsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";

        public string Convert(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                this.GetXlsContent(filePath, sb);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sb.ToString();
        }

        private void GetXlsContent(string filePath, StringBuilder sb)
        {
            var conString = string.Format(this.xlsConnectionString, filePath);

            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;
                        connExcel.Open();

                        DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        List<string> sheetNames = new List<string>();
                        for(var i = 0; i < dtExcelSchema.Rows.Count; i++)
                        {
                            sheetNames.Add(dtExcelSchema.Rows[i]["TABLE_NAME"].ToString());
                        }

                        foreach (var sheetName in sheetNames)
                        {
                            this.ExtractDataFromSheet(cmdExcel, odaExcel, sheetName, sb);
                        }
                        
                        connExcel.Close();
                    }
                }
            }
        }

        private void ExtractDataFromSheet(OleDbCommand cmdExcel, OleDbDataAdapter odaExcel, string sheetName, StringBuilder sb)
        {
            using (DataTable dt = new DataTable())
            {
                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                odaExcel.SelectCommand = cmdExcel;
                odaExcel.Fill(dt);

                if (dt.Columns.Count > 0)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append(column.Caption);
                        sb.Append("; ");
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if(row.ItemArray != null && row.ItemArray.Length > 0)
                        foreach(var item in row.ItemArray)
                        {
                                sb.Append(item.ToString());
                                sb.Append("; ");
                            }
                    }
                }
            }
        }
    }
}
