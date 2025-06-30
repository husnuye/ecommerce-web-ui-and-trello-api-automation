using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace WebTests.Utils
{
    public static class ExcelReader
    {
        /// <summary>
        /// Reads the first row of the first worksheet in the given Excel file.
        /// </summary>
        public static List<string> ReadFirstRow(string filePath)
        {
            var values = new List<string>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Excel file not found: {filePath}");

            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            var firstRow = worksheet.FirstRowUsed();

            foreach (var cell in firstRow.CellsUsed())
            {
                values.Add(cell.GetString());
            }

            return values;
        }

        /// <summary>
        /// Reads all values from a specific column (by index) in the given worksheet.
        /// </summary>
        public static List<string> ReadColumn(string filePath, int columnIndex)
        {
            var values = new List<string>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Excel file not found: {filePath}");

            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);

            foreach (var row in worksheet.RowsUsed())
            {
                var value = row.Cell(columnIndex).GetValue<string>();
                if (!string.IsNullOrWhiteSpace(value))
                    values.Add(value);
            }

            return values;
        }
    }
}