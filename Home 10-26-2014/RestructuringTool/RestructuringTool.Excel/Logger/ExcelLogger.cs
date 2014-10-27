using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace RestructuringTool.Excel.Logger
{
    public static class ExcelLogger
    {
        private static ConcurrentDictionary<int, ICollection<ExcelWarning>> _logDictionary = new ConcurrentDictionary<int, ICollection<ExcelWarning>>();

        public static void Log(IWorkbook book, string message, Level level, int? rowIndex = null, int? columnIndex = null, string sheetName = null)
        {
            int hasCode = book.GetHashCode();
            if (!_logDictionary.ContainsKey(hasCode))
            {
                _logDictionary[hasCode] = new List<ExcelWarning>();
            }

            _logDictionary[hasCode].Add(new ExcelWarning(message)
            {
                Level = level,
                RowIndex = rowIndex,
                ColumnIndex = columnIndex,
                SheetName = sheetName
            });
        }

        public static IEnumerable<ExcelWarning> GetWorkbookLog(IWorkbook book)
        {
            return _logDictionary[book.GetHashCode()] ?? Enumerable.Empty<ExcelWarning>();
        }

        public static IEnumerable<ExcelWarning> ClearWorkbookLog(IWorkbook book)
        {
            return _logDictionary[book.GetHashCode()] = null;
        }
    }
}
