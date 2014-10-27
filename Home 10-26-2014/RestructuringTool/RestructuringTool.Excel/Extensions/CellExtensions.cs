using System;
using NPOI.SS.UserModel;
using RestructuringTool.Excel.Logger;

namespace RestructuringTool.Excel.Extensions
{
    public static class CellExtensions
    {
        public static object GetCellValue(this ICell cell, CellType cellType)
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
        }

        public static object GetCellValueOrLog(this ICell cell, CellType cellType)
        {
            if (cell.CellType == cellType)
            {
                return GetCellValue(cell, cellType);
            }

            string message = string.Format("Cell (row {0} column {1}) on sheet {2} has incorrect type", cell.RowIndex, cell.ColumnIndex, cell.Sheet.SheetName);
            ExcelLogger.Log(cell.Sheet.Workbook, message, Level.Error);
            //Log about exception
            return null;
            //throw new InvalidOperationException("Cell has Incorrect type");
        }
    }
}
