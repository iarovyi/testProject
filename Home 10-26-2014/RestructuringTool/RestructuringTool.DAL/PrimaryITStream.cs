using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.DAL
{
    /*public enum PrimaryITStream
    {
        APAC_IC,
        FRDS,
        GPS_IT,
        GROUP_CIO,
        ID_IT,
        IS_IT,
        ITAM,
        O_and_CS,
        WMA_IT,
        WMCH
    }*/

    public class PrimaryITStream
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
