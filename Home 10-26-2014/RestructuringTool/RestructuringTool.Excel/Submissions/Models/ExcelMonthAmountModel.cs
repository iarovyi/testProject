using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestructuringTool.DAL;

namespace RestructuringTool.Excel.Submissions
{
    public class ExcelMonthAmountModel : ExcelAmountModel
    {
        public Month Month { get; set; }

        public int Year { get; set; }
    }
}
