using System;
using NPOI.SS.UserModel;

namespace RestructuringTool.Excel.Declerative
{
    public class ExcelAttribute : Attribute
    {
        public CellType Type { get; set; }
    }
}
