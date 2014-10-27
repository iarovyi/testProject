using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.Excel.Logger
{
    public class ExcelWarning
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
    }
}
