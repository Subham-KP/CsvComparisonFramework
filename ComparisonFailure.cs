
namespace CsvComparisonFramework
{
    // Represents a comparison failure with details
    public class ComparisonFailure
    {
        public string FailureType;    // Type of failure (Missing, Extra, Mismatch)
        public CsvRecord Record;      // The affected record
        public string[] FieldFailures;// Array of field-level failures
        public int FieldFailureCount; // Number of field failures

        public ComparisonFailure()
        {
            FieldFailures = new string[10]; // Fixed size, adjust as needed
            FieldFailureCount = 0;
        }

        public void AddFieldFailure(string failure)
        {
            if (FieldFailureCount < FieldFailures.Length)
            {
                FieldFailures[FieldFailureCount] = failure;
                FieldFailureCount++;
            }
        }
    }
}
