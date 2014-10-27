using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using RestructuringTool.Excel.Declerative;
using RestructuringTool.Excel.Logger;

namespace RestructuringTool.Excel.Submissions
{
    [ExcelRowsStartCell(Row = 6)]
    public class CarExcelModel //: IValidatableObject
    {
        public CarExcelModel()
        {
            Amounts = new List<ExcelAmountModel>();
        }

        [RowExcelCell(Column = 0, Type = CellType.String)]
        public string Name { get; set; }

        [RowExcelCell(Column = 1, Type = CellType.String)]
        public string Title { get; set; }

        public ICollection<ExcelAmountModel> Amounts { get; set; }
        public IEnumerable<ExcelWarning> Validate()
        {
            var warnings = new List<ExcelWarning>();

            foreach (ExcelTotalAmountModel totalAmount in Amounts.OfType<ExcelTotalAmountModel>())
            {
                var monthAmounts = Amounts.OfType<ExcelMonthAmountModel>().ToList();
                decimal sum = monthAmounts.Where(m =>
                {
                    var dateTime = new DateTime(m.Year, (int)m.Month, 1);
                    return totalAmount.From <= dateTime && dateTime <= totalAmount.To;
                }).Sum(a => a.Amount);

                if (sum != totalAmount.Amount)
                {
                    var message = string.Format("Total sum for car {0} was calculated incorrect ({1} instead {2}) for period since {3} to {4}",
                        Name, totalAmount.Amount, sum, totalAmount.From.ToShortDateString(), totalAmount.To.ToShortDateString());
                    warnings.Add(new ExcelWarning(message) { Level = Level.Warning });
                }
            }

            return warnings;
        }
    }
}
