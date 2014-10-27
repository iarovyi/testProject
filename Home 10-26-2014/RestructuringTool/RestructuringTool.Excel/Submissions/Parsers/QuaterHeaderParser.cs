using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestructuringTool.Excel.Submissions
{
    public class QuaterHeaderParser : IHeaderDateParser
    {
        private static string quoterRegex = @"^(\d{1})Q\s(\d{4})";
        public bool CanParse(string input)
        {
            return Regex.IsMatch(input, quoterRegex);
        }

        public ExcelAmountModel Parse(string headerString, double amount)
        {
            Match match = Regex.Match(headerString, quoterRegex);
            int quoter = int.Parse(match.Groups[1].Captures[0].Value);
            int year = int.Parse(match.Groups[2].Captures[0].Value);

            return new ExcelTotalAmountModel()
            {
                Amount = (decimal)amount,
                From = new DateTime(year, quoter * 3 - 2, 1),
                To = new DateTime(year, quoter * 3, 28)
            };
        }
    }
}
