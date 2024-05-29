using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{

    public class CT_CSV
    {
        public CT_CSV()
        {
        }

        public static DataTable Load_CSVData(string FilePath, Encoding OpenEncoding)
        {
            DataTable dataTable;
            DataTable Result = null;
            if (!File.Exists(FilePath))
            {
                return null;
            }
            CsvFileReader reader = new CsvFileReader(FilePath, OpenEncoding, EmptyLineBehavior.NoColumns);
            try
            {
                Result = new DataTable();
                List<string> columns = new List<string>();
                if (!reader.ReadRow(columns))
                {
                    dataTable = null;
                }
                else
                {
                    foreach (string TargetColumn in columns)
                    {
                        Result.Columns.Add(TargetColumn);
                    }
                    columns.Clear();
                    while (reader.ReadRow(columns))
                    {
                        Result.Rows.Add(columns.ToArray());
                    }
                    reader.Dispose();
                    dataTable = Result;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                reader.Dispose();
                Console.WriteLine(string.Format("Error reading from {0}.\r\n\r\n{1}", FilePath, ex.Message));
                dataTable = null;
            }
            return dataTable;
        }

        public static bool Save_CSVData(DataTable InputData, string FilePath, bool Append, Encoding SaveEncoding)
        {
            bool flag;
            CsvFileWriter writer = new CsvFileWriter(FilePath, SaveEncoding, Append);
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn TargetColumn in InputData.Columns)
                {
                    columns.Add(TargetColumn.ColumnName);
                }
                writer.WriteRow(columns);
                columns.Clear();
                foreach (DataRow row in InputData.Rows)
                {
                    object[] itemArray = row.ItemArray;
                    for (int i = 0; i < (int)itemArray.Length; i++)
                    {
                        object TargetItem = itemArray[i];
                        if (TargetItem != null)
                        {
                            columns.Add(TargetItem.ToString());
                        }
                        else
                        {
                            columns.Add("");
                        }
                    }
                    writer.WriteRow(columns);
                    columns.Clear();
                }
                writer.Dispose();
                flag = true;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                writer.Dispose();
                Console.WriteLine(string.Format("Error writing to {0}.\r\n\r\n{1}", FilePath, ex.Message));
                flag = false;
            }
            return flag;
        }
    }
}
