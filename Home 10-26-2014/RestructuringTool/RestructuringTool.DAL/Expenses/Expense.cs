using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    public class Expense
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string GPN { get; set; }

        public EmployeeRank EmployeeRank { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public int EffortByRole { get; set; }

        #region Program
        public virtual Program Program { get; set; }

        [ForeignKey("Program")]
        public Guid ProgramId { get; set; }
        #endregion

        public string eXtractId { get; set; }

        public ExpenseCostType CostType { get; set; }

        public Region Region { get; set; }

        public int LegalEntity { get; set; }

        #region Salary
        public decimal SalaryCostForPerson { get; set; }

        [NotMapped]
        public decimal SalaryCostPerYear
        {
            get { return SalaryCostPerMonth*12; }
        }

        [NotMapped]
        public decimal SalaryCostPerMonth
        {
            get { return SalaryCostForPerson/1000000; }
        }

        [NotMapped]
        public decimal DailyRate
        {
            get { return SalaryCostForPerson / 20; }
        }
        #endregion

        public virtual ICollection<MonthExpense> Expenses { get; set; }

        #region Autocalculated Expenses
        [NotMapped]
        public decimal Expenses1stQuater
        {
            get { return Expenses.Where(e => (int) e.Month > 0 && (int)e.Month < 4 && e.Year == DateTime.Now.Year).Sum(e => e.Amount); }
        }

        [NotMapped]
        public decimal Expenses2stQuater
        {
            get { return Expenses.Where(e => (int)e.Month > 3 && (int)e.Month < 7 && e.Year == DateTime.Now.Year).Sum(e => e.Amount); }
        }

        [NotMapped]
        public decimal Expenses3stQuater
        {
            get { return Expenses.Where(e => (int)e.Month > 6 && (int)e.Month < 10 && e.Year == DateTime.Now.Year).Sum(e => e.Amount); }
        }

        [NotMapped]
        public decimal Expenses4stQuater
        {
            get { return Expenses.Where(e => (int)e.Month > 9 && (int)e.Month < 13 && e.Year == DateTime.Now.Year).Sum(e => e.Amount); }
        }

        [NotMapped]
        public decimal ExpensesFor1stYearHalf
        {
            get { return Expenses1stQuater + Expenses2stQuater; }
        }

        [NotMapped]
        public decimal ExpensesFor2stYearHalf
        {
            get { return Expenses3stQuater + Expenses4stQuater; }
        }

        [NotMapped]
        public decimal ExpensesForThisYear
        {
            get { return ExpensesFor1stYearHalf + ExpensesFor2stYearHalf; }
        }
        #endregion

        public string Comments { get; set; }

        #region KTLO and CTB
        public decimal CTB { get; set; }

        [NotMapped]
        public decimal KTLO 
        {
            get { return 1 - CTB; }
        }

        [NotMapped]
        public decimal CTBinMillions
        {
            get { return CTB*ExpensesForThisYear; }
        }

        [NotMapped]
        public decimal KTLOinMillions
        {
            get { return KTLO * ExpensesForThisYear; }
        }
        #endregion
    }
}
