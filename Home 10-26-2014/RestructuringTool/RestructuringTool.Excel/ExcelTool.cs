using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestructuringTool.Excel.Submissions;

namespace RestructuringTool.Excel
{
    public static class ExcelTool
    {
        public static HumanExcelModel ParseSubmissions(string filePath)
        {
            return SubmissionsTool.ParseExcel(filePath);
        }
    }
}
