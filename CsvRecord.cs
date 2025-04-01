
namespace CsvComparisonFramework
{
    // Represents a single CSV record with its fields
    public class CsvRecord
    {
        public string[] FieldNames;  // Array of field names
        public string[] FieldValues; // Array of field values

        public CsvRecord(int fieldCount)
        {
            FieldNames = new string[fieldCount];
            FieldValues = new string[fieldCount];
        }

        // Generates composite key from specified key fields
        public string GetCompositeKey(string[] keyFields)
        {
            string result = "";
            for (int i = 0; i < keyFields.Length; i++)
            {
                for (int j = 0; j < FieldNames.Length; j++)
                {
                    if (FieldNames[j] == keyFields[i])
                    {
                        result += FieldValues[j];
                        if (i < keyFields.Length - 1) result += "|";
                        break;
                    }
                }
            }
            return result;
        }
    }
}
