﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestructuringTool.Excel.Submissions
{
    public class ExcelTotalAmountModel : ExcelAmountModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
