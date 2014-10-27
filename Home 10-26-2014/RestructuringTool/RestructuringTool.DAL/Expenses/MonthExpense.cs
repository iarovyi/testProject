using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    public class MonthExpense
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        #region Expense TODO: replace it with key based on two fields.
        public virtual Expense Expense { get; set; }

        [ForeignKey("Expense")]
        public Guid ExpenseId { get; set; }
        #endregion

        public Month Month { get; set; }

        public decimal Amount { get; set; }

        public int Year { get; set; }
    }
}
