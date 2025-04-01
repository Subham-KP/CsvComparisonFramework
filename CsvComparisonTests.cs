using System;

namespace CsvComparisonFramework
{

    public class CsvComparisonTests
    {
        private const int MAX_RECORDS = 100;
        private const int MAX_FAILURES = 100;

        public static void RunTests()
        {
            Console.WriteLine("Running Unit Tests...");

            TestBasicDifferences();
            TestEmptyActualFile();
            TestMultipleFieldDifferences();
            TestCompositeKeyComparison();

            Console.WriteLine("\nAll tests completed!");
        }

        private static void TestBasicDifferences()
        {
            Console.WriteLine("\nTest 1: Basic Differences");
            string[] expectedData = new string[] {
            "BAII,SCOPE,VALUE,DATE",
            "\"001625\",\"533.5761748850723\",\"1000\",\"2025-03-31\"",
            "\"001626\",\"123.456\",\"2000\",\"2025-03-31\"",
            "\"001627\",\"789.012\",\"3000\",\"2025-03-31\""
        };
            string[] actualData = new string[] {
            "BAII,SCOPE,VALUE,DATE",
            "\"001625\",\"533.9761748850723\",\"1000\",\"2025-03-31\"",
            "\"001628\",\"999.999\",\"4000\",\"2025-03-31\"",
            "\"001626\",\"123.456\",\"2000\",\"2025-03-31\""
        };

            int failureCount;
            var failures = CsvComparator.Compare(
                expectedData, actualData, new[] { "BAII" }, MAX_RECORDS, MAX_FAILURES, out failureCount
            );

            Assert(failureCount == 3, "Should find 3 differences");
        }

        private static void TestEmptyActualFile()
        {
            Console.WriteLine("\nTest 2: Empty Actual File");
            string[] expectedData = new string[] {
            "ID,NAME,AMOUNT",
            "\"1\",\"John\",\"100.50\"",
            "\"2\",\"Jane\",\"200.75\""
        };
            string[] actualData = new string[] { "ID,NAME,AMOUNT" };

            int failureCount;
            var failures = CsvComparator.Compare(
                expectedData, actualData, new[] { "ID" }, MAX_RECORDS, MAX_FAILURES, out failureCount
            );

            Assert(failureCount == 2, "Should find 2 missing records");
        }

        private static void TestMultipleFieldDifferences()
        {
            Console.WriteLine("\nTest 3: Multiple Field Differences");
            string[] expectedData = new string[] {
            "PRODUCT_ID,PRICE,STOCK,STATUS",
            "\"P001\",\"99.99\",\"50\",\"Active\"",
            "\"P002\",\"149.50\",\"25\",\"Inactive\""
        };
            string[] actualData = new string[] {
            "PRODUCT_ID,PRICE,STOCK,STATUS",
            "\"P001\",\"100.00\",\"45\",\"Inactive\"",
            "\"P002\",\"149.50\",\"25\",\"Inactive\""
        };

            int failureCount;
            var failures = CsvComparator.Compare(
                expectedData, actualData, new[] { "PRODUCT_ID" }, MAX_RECORDS, MAX_FAILURES, out failureCount
            );

            Assert(failureCount == 1, "Should find 1 record with differences");
            Assert(failures[0].FieldFailureCount == 3, "Should find 3 field differences");
        }

        private static void TestCompositeKeyComparison()
        {
            Console.WriteLine("\nTest 4: Composite Key Comparison");
            string[] expectedData = new string[] {
            "ORDER_ID,ITEM_ID,QUANTITY,PRICE",
            "\"O1\",\"I1\",\"5\",\"10.00\"",
            "\"O1\",\"I2\",\"3\",\"15.00\"",
            "\"O2\",\"I1\",\"2\",\"10.00\""
        };
            string[] actualData = new string[] {
            "ORDER_ID,ITEM_ID,QUANTITY,PRICE",
            "\"O1\",\"I1\",\"5\",\"10.00\"",
            "\"O2\",\"I2\",\"1\",\"20.00\"",
            "\"O2\",\"I1\",\"2\",\"10.00\""
        };

            int failureCount;
            var failures = CsvComparator.Compare(
                expectedData, actualData, new[] { "ORDER_ID", "ITEM_ID" }, MAX_RECORDS, MAX_FAILURES, out failureCount
            );

            Assert(failureCount == 2, "Should find 2 differences with composite key");
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                Console.WriteLine("FAIL: " + message);
                throw new Exception(message);
            }
            else
            {
                Console.WriteLine("PASS: " + message);
            }
        }

    }
}
