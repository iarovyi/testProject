using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace RestructuringTool.Excel.Extensions
{
    public static class SheetExtensions
    {
        public static IEnumerable<IRow> GetRowsFromWhile(this ISheet sheet, int startRowIndex, Func<IRow, bool> predicate)
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
        }
    }
}
