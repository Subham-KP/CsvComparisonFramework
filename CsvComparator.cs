using System;

namespace CsvComparisonFramework
{

    public class CsvComparator
    {
        public static ComparisonFailure[] Compare(
            string[] expectedLines,
            string[] actualLines,
            string[] keyFields,
            int maxRecords,
            int maxFailures,
            out int failureCount)
        {
            ComparisonFailure[] failures = new ComparisonFailure[maxFailures];
            failureCount = 0;

            // Read CSV data into arrays
            int expectedCount;
            CsvRecord[] expectedRecords = CsvReader.ReadCsv(expectedLines, maxRecords, out expectedCount);
            int actualCount;
            CsvRecord[] actualRecords = CsvReader.ReadCsv(actualLines, maxRecords, out actualCount);

            // Check for missing in actual
            for (int i = 0; i < expectedCount; i++)
            {
                string expectedKey = expectedRecords[i].GetCompositeKey(keyFields);
                bool found = false;
                for (int j = 0; j < actualCount; j++)
                {
                    if (actualRecords[j].GetCompositeKey(keyFields) == expectedKey)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found && failureCount < maxFailures)
                {
                    failures[failureCount] = new ComparisonFailure
                    {
                        FailureType = "Missing in Actual",
                        Record = expectedRecords[i]
                    };
                    failureCount++;
                }
            }

            // Check for extra in actual and field mismatches
            for (int i = 0; i < actualCount; i++)
            {
                string actualKey = actualRecords[i].GetCompositeKey(keyFields);
                bool found = false;
                int expectedIndex = -1;
                for (int j = 0; j < expectedCount; j++)
                {
                    if (expectedRecords[j].GetCompositeKey(keyFields) == actualKey)
                    {
                        found = true;
                        expectedIndex = j;
                        break;
                    }
                }
                if (!found && failureCount < maxFailures)
                {
                    failures[failureCount] = new ComparisonFailure
                    {
                        FailureType = "Extra in Actual",
                        Record = actualRecords[i]
                    };
                    failureCount++;
                }
                else if (found && failureCount < maxFailures)
                {
                    ComparisonFailure failure = CompareRecords(expectedRecords[expectedIndex], actualRecords[i]);
                    if (failure.FieldFailureCount > 0)
                    {
                        failure.FailureType = "Field Mismatch";
                        failure.Record = actualRecords[i];
                        failures[failureCount] = failure;
                        failureCount++;
                    }
                }
            }

            // Log failures
            for (int i = 0; i < failureCount; i++)
            {
                Console.WriteLine(FormatFailure(failures[i], keyFields));
            }

            return failures;
        }

        private static ComparisonFailure CompareRecords(CsvRecord expected, CsvRecord actual)
        {
            ComparisonFailure failure = new ComparisonFailure();
            for (int i = 0; i < expected.FieldNames.Length; i++)
            {
                if (expected.FieldValues[i] != actual.FieldValues[i])
                {
                    failure.AddFieldFailure(
                        "Failed Fieldname: " + expected.FieldNames[i] +
                        " Expected Input Value: \"" + expected.FieldValues[i] +
                        "\" | Actual Value: \"" + actual.FieldValues[i] + "\""
                    );
                }
            }
            return failure;
        }

        private static string FormatFailure(ComparisonFailure failure, string[] keyFields)
        {
            string keyField = keyFields[0];
            string keyValue = "";
            for (int i = 0; i < failure.Record.FieldNames.Length; i++)
            {
                if (failure.Record.FieldNames[i] == keyField)
                {
                    keyValue = failure.Record.FieldValues[i];
                    break;
                }
            }
            string output = "Failure Type: " + failure.FailureType +
                            " for record having unique field Name: " + keyField +
                            " with value: \"" + keyValue + "\"";
            for (int i = 0; i < failure.FieldFailureCount; i++)
            {
                output += "\n" + failure.FieldFailures[i];
            }
            return output;
        }
    }
}
