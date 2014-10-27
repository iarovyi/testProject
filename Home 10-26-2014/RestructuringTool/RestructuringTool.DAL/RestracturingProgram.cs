using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    public class RestracturingProgram
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public string Program { get; set; }

        public string LineItem { get; set; }

        #region Division
        public virtual OperatingCommitteeDivision Division { get; set; }

        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }
        #endregion

        #region Operating Committee
        public virtual OperatingCommittee OperatingCommittee { get; set; } //make it class

        [ForeignKey("OperatingCommittee")]
        public Guid OperatingCommitteeId { get; set; }
        #endregion

        #region PrimaryITStream
        public virtual PrimaryITStream PrimaryITStream { get; set; }

        [ForeignKey("PrimaryITStream")]
        public Guid PrimaryITStreamId { get; set; }
        #endregion

        public string Description { get; set; }

        public string RationaleForIncludion { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
