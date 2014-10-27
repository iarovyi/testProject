using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestructuringTool.DAL;

namespace RestructuringTool.Excel.Submissions
{
    public class MonthHeaderParser : IHeaderDateParser
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
    }
}
