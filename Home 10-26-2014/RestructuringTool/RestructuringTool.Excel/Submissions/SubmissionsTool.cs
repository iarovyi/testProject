using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RestructuringTool.Excel.Declerative;
using RestructuringTool.Excel.Extensions;
using RestructuringTool.Excel.Logger;

namespace RestructuringTool.Excel.Submissions
{
    public static class SubmissionsTool
    {
        private static string sheetName = "Sheet1";
        private const int CalendarHeaderStartColumnIndex = 2;
        private const int CalendarHeaderRowIndex = 5;

        private static readonly IEnumerable<IHeaderDateParser> HeaderDateParsers = new List<IHeaderDateParser>()
        {
            new MonthHeaderParser(), new QuaterHeaderParser(), new FullYearHeaderParser()
        };

        public static HumanExcelModel ParseExcel(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                var workbook = new XSSFWorkbook(fileStream);
                var result = ParseExcel(workbook);
                var log = ExcelLogger.GetWorkbookLog(workbook);
                return result;
            }
        }

        private static HumanExcelModel ParseExcel(XSSFWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet(sheetName);

            #region Parse Cars
            int carsStartRowIndex = DeclerativeExtensions.GetExcelRowsStartRowIndex<CarExcelModel>();
            IEnumerable<IRow> carRows = sheet.GetRowsFromWhile(carsStartRowIndex, r => r != null).ToList();
            IList<CarExcelModel> cars = carRows.ParseExcelRows<CarExcelModel>();
            int carsEndRowIndex = carsStartRowIndex + cars.Count;

            foreach (ICell headerCell in GetCalendarHeaderCells(sheet))
            {
                for (int carIndex = 0, rowIndex = carsStartRowIndex; rowIndex < carsEndRowIndex; rowIndex++, carIndex++)
                {
                    ParseAmount(headerCell, sheet.GetRow(rowIndex).GetCell(headerCell.ColumnIndex), cars[carIndex]);
                }
            }
            #endregion

            IEnumerable<ExcelWarning> errors = cars.SelectMany(c => c.Validate()).ToList();

            #region Parse Human
            HumanExcelModel human = sheet.ParseAbsoluteExcelObject<HumanExcelModel>();
            human.Cars = cars;
            #endregion

            return human;
        }

        private static IEnumerable<ICell> GetCalendarHeaderCells(ISheet sheet)
        {
            IRow headerRow = sheet.GetRow(CalendarHeaderRowIndex);
            for (int i = CalendarHeaderStartColumnIndex; i <= headerRow.LastCellNum; i++)
            {
                ICell cell = headerRow.GetCell(i);
                if (cell != null &&
                    cell.CellType == CellType.String &&
                    HeaderDateParsers.FirstOrDefault(p => p.CanParse(cell.StringCellValue)) != null)
                {
                    yield return cell;
                }
            }
        }

        private static void ParseAmount(ICell headerCell, ICell amountCell, CarExcelModel car)
        {
            IHeaderDateParser parser = HeaderDateParsers.FirstOrDefault(p => p.CanParse(headerCell.StringCellValue));
            //TODO: if parser is null

            ExcelAmountModel parsed = parser.Parse(headerCell.StringCellValue, amountCell.NumericCellValue);
            car.Amounts.Add(parsed);
        }
    }
}
