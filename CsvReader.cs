

namespace CsvComparisonFramework
{

    // Custom implementation without System.IO
    public class CsvReader
    {
        // Simplified: Assumes files are provided as string arrays (lines)
        public static CsvRecord[] ReadCsv(string[] lines, int maxRecords, out int recordCount)
        {
            CsvRecord[] records = new CsvRecord[maxRecords];
            recordCount = 0;
            string[] headers = null;

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                string[] values = ParseCsvLine(lines[i]);
                if (i == 0)
                {
                    headers = values;
                }
                else
                {
                    CsvRecord record = new CsvRecord(headers.Length);
                    for (int j = 0; j < headers.Length; j++)
                    {
                        record.FieldNames[j] = headers[j];
                        record.FieldValues[j] = values[j];
                    }
                    records[recordCount] = record;
                    recordCount++;
                }
            }
            return records;
        }

        // Custom CSV line parser without LINQ
        private static string[] ParseCsvLine(string line)
        {
            int count = 1;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',') count++;
            }

            string[] result = new string[count];
            int index = 0;
            string current = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"') continue; // Simple quote removal
                if (line[i] == ',')
                {
                    result[index] = current;
                    index++;
                    current = "";
                }
                else
                {
                    current += line[i];
                }
            }
            result[index] = current;
            return result;
        }
    }
}
