using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    public class OperatingCommittee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        #region Division
        /*[ForeignKey("Division")]
        public Guid DivisionId { get; set; }
        public virtual OperatingCommitteeDivision Division { get; set; }*/
        #endregion

        public string Name { get; set; }
    }

    /*public enum OC
    {
        CC_Strategic_Regulatory,
        CC_ICG_Other,
        Comms_and_Branding,
        Compliance_OC,
        CREAS_OC,
        Finance_Change_Committee,
        Group_Operations_ICG,
        HR_OC,
        IPS,
        IT_OC,
        Legal_OC,
        Non_Core,
        Legacy,
        Operations,
        Shared_Services,
        Risk_Change_Broad,
        SB,
        Strategic_Data_Services,
        Supply_and_Demand_Management_OC,
        WM_Europe_and_Shared
    }*/
}
