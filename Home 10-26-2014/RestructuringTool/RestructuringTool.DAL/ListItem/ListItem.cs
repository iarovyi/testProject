using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    public class ListItem
    {
        public string Id { get; set; }

        public ListItemStatus Status { get; set; }

        public string LineItem { get; set; }

        #region ListItemCostType
        public virtual ListItemCostType CostType { get; set; }

        [ForeignKey("CostType")]
        public Guid CostTypeId { get; set; }
        #endregion

        #region Program
        public virtual Program Program { get; set; }

        [ForeignKey("Program")]
        public Guid ProgramId { get; set; }
        #endregion

        public string eXtractId { get; set; }

        public ExpenseCostType RestracturingCostType { get; set; }

        #region Division
        public virtual OperatingCommitteeDivision Division { get; set; }

        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }
        #endregion

        #region OperatingCommittee
        public OperatingCommittee OperatingCommittee { get; set; }

        [ForeignKey("OperatingCommittee")]
        public Guid OperatingCommitteeId { get; set; }
        #endregion

        #region PrimaryITStream
        public virtual PrimaryITStream PrimaryITStream { get; set; }

        [ForeignKey("PrimaryITStream")]
        public Guid PrimaryITStreamId { get; set; }
        #endregion

        public int LegalEntity { get; set; }

        public string Comments { get; set; }

        public decimal CTB { get; set; }

        public decimal KTLO
        {
            get { return 1 - CTB; }
        }

        public decimal CTBinMillions
        {
            get { return CTB * ExpensesForThisYear; }
        }

        public decimal KTLOinMillions
        {
            get { return KTLO * ExpensesForThisYear; }
        }



        public decimal ExpensesForThisYear { get; set; }//???????????????????????/
    }
}
