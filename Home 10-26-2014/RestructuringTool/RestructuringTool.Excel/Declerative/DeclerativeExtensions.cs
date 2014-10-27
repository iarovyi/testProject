using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;
using RestructuringTool.Excel.Extensions;

namespace RestructuringTool.Excel.Declerative
{
    public static class DeclerativeExtensions
    {
        public static T ParseAbsoluteExcelObject<T>(this ISheet sheet) where T : new()
        {
            T excelModel = new T();

            var properties = typeof(T).GetProperties().Where(p => p.CustomAttributes.Count(a => a.AttributeType == typeof(AbsoluteExcelCellAttribute)) == 1).ToList();
            foreach (var propertyInfo in properties)
            {
                var attr = (AbsoluteExcelCellAttribute)propertyInfo.GetCustomAttributes(typeof(AbsoluteExcelCellAttribute), false).First();
                ICell cell = sheet.GetRow(attr.Row).GetCell(attr.Column);
                object value = cell.GetCellValueOrLog(attr.Type);
                propertyInfo.SetValue(excelModel, value);
            }

            return excelModel;
        }

        public static IList<T> ParseExcelRows<T>(this IEnumerable<IRow> rows) where T : new()
        {
            var excelModels = new List<T>();

            var properties =
                typeof(T).GetProperties()
                    .Where(p => p.CustomAttributes.Count(a => a.AttributeType == typeof(RowExcelCellAttribute)) == 1)
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
                    object value = cell.GetCellValueOrLog(property.ExcelAttr.Type);
                    property.PropertyInfo.SetValue(rowModel, value);
                }
                excelModels.Add(rowModel);
            }

            return excelModels;
        }

        public static int GetExcelRowsStartRowIndex<T>()
        {
            object[] attrs = typeof(T).GetCustomAttributes(typeof(ExcelRowsStartCellAttribute), false);
            if (attrs.Count() != 1)
            {
                throw new InvalidOperationException(string.Format("Type {0} has invalid count of ExcelRowsStartCellAttribute", typeof(T).Name));
            }

            return ((ExcelRowsStartCellAttribute)attrs.Single()).Row;
        }
    }
}
