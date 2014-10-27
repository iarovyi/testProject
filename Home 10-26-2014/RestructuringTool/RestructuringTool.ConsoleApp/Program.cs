using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RestructuringTool.DAL;
using RestructuringTool.DAL.Expenses;
using RestructuringTool.Excel;
using Match = System.Text.RegularExpressions.Match;

namespace RestructuringTool.ConsoleApp
{
    class Program
    {
        private static string inputFilePath = @"C:\Users\Sergii\Desktop\input.xlsx";
        private static string ouputFilePath = @"C:\Users\Sergii\Desktop\ouput.xlsx";


        
        static void Main(string[] args)
        {
            var submissions = ExcelTool.ParseSubmissions(inputFilePath);

            /*using (var fileStream = File.OpenRead(inputFilePath))
            {
                ParseExcel(new XSSFWorkbook(fileStream));
            }*/

            /*using (var dbContext = new RestructuringContext())
            {
                var programs = dbContext.RestracturingPrograms.ToList();
            }*/
            Console.ReadKey();
        }

        


        /*private static object GetCellValue(ICell cell, CellType cellType)
        {
            switch (cellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.RichStringCellValue;
                default:
                    throw new InvalidOperationException(string.Format("Excel cell  on row {0} column {1} on sheet {2}  has incorrect type", cell.RowIndex, cell.ColumnIndex, cell.Sheet.SheetName));
            }
        }*/

        /*private static object GetCellValueOrLog(ICell cell, CellType cellType)
        {
            if (cell.CellType == cellType)
            {
                return GetCellValue(cell, cellType);
            }

            //Log about exception
            return null;
        }*/

        /*private static T ParseAbsoluteExcelObject<T>(ISheet sheet) where T : new()
        {
            T excelModel = new T();

            var properties = typeof(T).GetProperties().Where(p => p.CustomAttributes.Count(a => a.AttributeType == typeof(AbsoluteExcelCellAttribute)) == 1).ToList();
            foreach (var propertyInfo in properties)
            {
                var attr = (AbsoluteExcelCellAttribute)propertyInfo.GetCustomAttributes(typeof(AbsoluteExcelCellAttribute), false).First();
                ICell cell = sheet.GetRow(attr.Row).GetCell(attr.Column);
                object value = GetCellValueOrLog(cell, attr.Type);
                propertyInfo.SetValue(excelModel, value);
            }

            return excelModel;
        }

        private static IList<T> ParseExcelRows<T>(IEnumerable<IRow> rows) where T : new()
        {
            var excelModels = new List<T>();

            var properties =
                typeof (T).GetProperties()
                    .Where(p => p.CustomAttributes.Count(a => a.AttributeType == typeof (RowExcelCellAttribute)) == 1)
                    .Select(pi => new
                    {
                        PropertyInfo = pi,
                        ExcelAttr = (RowExcelCellAttribute)pi.GetCustomAttributes(typeof(RowExcelCellAttribute), false).Single()
                    }).ToList();

            foreach (IRow row in rows)
            {
                T rowModel = new T();
                foreach (var property in properties)
                {
                    ICell cell = row.GetCell(property.ExcelAttr.Column);
                    object value = GetCellValueOrLog(cell, property.ExcelAttr.Type);
                    property.PropertyInfo.SetValue(rowModel, value);
                }
                excelModels.Add(rowModel);
            }

            return excelModels;
        }

        private static int GetExcelRowsStartRowIndex<T>()
        {
            object[] attrs = typeof (T).GetCustomAttributes(typeof (ExcelRowsStartCellAttribute), false);
            if (attrs.Count() != 1)
            {
                throw new InvalidOperationException(string.Format("Type {0} has invalid count of ExcelRowsStartCellAttribute", typeof(T).Name));
            }

            return ((ExcelRowsStartCellAttribute) attrs.Single()).Row;
        }*/

        /*private static IEnumerable<IRow> GetRowsFromWhile(int startRowIndex, ISheet sheet, Func<IRow, bool> predicate)
        {
            if (startRowIndex >= sheet.PhysicalNumberOfRows)
            {
                yield break;
            }

            for (int i = startRowIndex; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (!predicate(row))
                {
                    yield break;
                }
                yield return row;
            }
        }*/

        /*private static string sheetName = "Sheet1";
        private const int CalendarHeaderStartColumnIndex = 2;
        private const int CalendarHeaderRowIndex = 5;
        
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

        private static void ParseExcel(XSSFWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet(sheetName);

            #region Parse Cars
            int carsStartRowIndex = GetExcelRowsStartRowIndex<CarExcelModel>();
            IEnumerable<IRow> carRows = GetRowsFromWhile(carsStartRowIndex, sheet, r => r != null).ToList();
            IList<CarExcelModel> cars = ParseExcelRows<CarExcelModel>(carRows);
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
            HumanExcelModel human = ParseAbsoluteExcelObject<HumanExcelModel>(sheet);
            human.Cars = cars;
            #endregion
        }


        private static readonly IEnumerable<IHeaderDateParser> HeaderDateParsers = new List<IHeaderDateParser>()
        {
            new MonthHeaderParser(), new QuaterHeaderParser(), new FullYearHeaderParser()
        };*/
    }


    /*public interface IHeaderDateParser
    {
        bool CanParse(string input);

        ExcelAmountModel Parse(string headerString, double amount);
    }*/

    /*public class MonthHeaderParser : IHeaderDateParser
    {
        public bool CanParse(string input)
        {
            DateTime _;
            return DateTime.TryParse(input, out _);
        }

        public ExcelAmountModel Parse(string headerString, double amount)
        {
            DateTime dateTime = DateTime.Parse(headerString);
            return new ExcelMonthAmountModel()
            {
                Amount = (decimal)amount,
                Month = (Month)dateTime.Month, //TODO: check that dateTime.Month starts from 1
                Year = dateTime.Year
            };
        }
    }*/

    /*public class QuaterHeaderParser : IHeaderDateParser
    {
        private static string quoterRegex = @"^(\d{1})Q\s(\d{4})";
        public bool CanParse(string input)
        {
            return Regex.IsMatch(input, quoterRegex);
        }

        public ExcelAmountModel Parse(string headerString, double amount)
        {
            Match match = Regex.Match(headerString, quoterRegex);
            int quoter = int.Parse(match.Groups[1].Captures[0].Value);
            int year = int.Parse(match.Groups[2].Captures[0].Value);

            return new ExcelTotalAmountModel()
            {
                Amount = (decimal)amount,
                From = new DateTime(year, quoter * 3 - 2, 1),
                To = new DateTime(year, quoter * 3, 28)
            };
        }
    }*/

    /*public class FullYearHeaderParser : IHeaderDateParser
    {
        private static string fullYearRegex = @"^(FY)\s\(d{4})";

        public bool CanParse(string input)
        {
            return Regex.IsMatch(input, fullYearRegex);
        }

        public ExcelAmountModel Parse(string headerString, double amount)
        {
            Match match = Regex.Match(headerString, fullYearRegex);
            int year = int.Parse(match.Groups[2].Captures[0].Value);

            return new ExcelTotalAmountModel()
            {
                Amount = (decimal)amount,
                From = new DateTime(year, 1, 1),
                To = new DateTime(year, 12, 28)
            };
        }
    }*/

    /*public class ExcelCalendarHeaderModel : List<ExcelAmountModel>
    {
        
    }*/

    /*public class ExcelAmountModel
    {
        public decimal Amount { get; set; }

        public int Column { get; set; }
    }*/

    /*public class ExcelMonthAmountModel : ExcelAmountModel
    {
        public Month Month { get; set; }

        public int Year { get; set; }
    }*/

    /*public class ExcelTotalAmountModel : ExcelAmountModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }*/

    

    /*public class HumanExcelModel
    {
        [AbsoluteExcelCell(Column = 1, Row = 0, Type = CellType.String)]
        public string Name { get; set; }

        [AbsoluteExcelCell(Column = 1, Row = 1, Type = CellType.String)]
        public string SecondName { get; set; }

        [AbsoluteExcelCell(Column = 1, Row = 2, Type = CellType.String)]
        public string Title { get; set; }

        public IList<CarExcelModel> Cars { get; set; }
    }

    [ExcelRowsStartCell(Row = 6)]
    public class CarExcelModel //: IValidatableObject
    {
        public CarExcelModel()
        {
            Amounts = new List<ExcelAmountModel>();
        }

        [RowExcelCell(Column = 0, Type = CellType.String)]
        public string Name { get; set; }

        [RowExcelCell(Column = 1, Type = CellType.String)]
        public string Title { get; set; }

        public ICollection<ExcelAmountModel> Amounts { get; set; }
        public IEnumerable<ExcelWarning> Validate()
        {
            var warnings = new List<ExcelWarning>();
            
            foreach (ExcelTotalAmountModel totalAmount in Amounts.OfType<ExcelTotalAmountModel>())
            {
                var monthAmounts = Amounts.OfType<ExcelMonthAmountModel>().ToList();
                decimal sum = monthAmounts.Where(m =>
                {
                    var dateTime = new DateTime(m.Year, (int) m.Month, 1);
                    return totalAmount.From <= dateTime  && dateTime <= totalAmount.To;
                }).Sum(a => a.Amount);

                if (sum != totalAmount.Amount)
                {
                    var message = string.Format("Total sum for car {0} was calculated incorrect ({1} instead {2}) for period since {3} to {4}",
                        Name, totalAmount.Amount, sum, totalAmount.From.ToShortDateString(), totalAmount.To.ToShortDateString());
                    warnings.Add(new ExcelWarning(message) { Level = Level.Warning });
                }
            }

            return warnings;
        }
    }*/

    /*public static class ExcelLogger
    {
        private static ConcurrentDictionary<string, ICollection<ExcelWarning>> _logDictionary = new ConcurrentDictionary<string, ICollection<ExcelWarning>>();

        public static void Log(string excelFile, string message, Level level, int? rowIndex = null, int? columnIndex = null, string sheetName = null)
        {
            
        }

        
    }*/

    /*public enum Level
    {
        Error   = 0,
        Warning = 1
    }*/

    /*public class ExcelWarning
    {
        public ExcelWarning(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        public Level Level { get; set; }

        public int? RowIndex { get; set; }

        public int? ColumnIndex { get; set; }

        public string SheetName { get; set; }
    }*/


   


    

    

    
}
