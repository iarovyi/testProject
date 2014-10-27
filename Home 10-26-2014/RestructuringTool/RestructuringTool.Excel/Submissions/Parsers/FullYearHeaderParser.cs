using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestructuringTool.Excel.Submissions
{
    public class FullYearHeaderParser : IHeaderDateParser
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
    }
}
