using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using RestructuringTool.Excel.Declerative;

namespace RestructuringTool.Excel.Submissions
{
    public class HumanExcelModel
    {
        [AbsoluteExcelCell(Column = 1, Row = 0, Type = CellType.String)]
        public string Name { get; set; }

        [AbsoluteExcelCell(Column = 1, Row = 1, Type = CellType.String)]
        public string SecondName { get; set; }

        [AbsoluteExcelCell(Column = 1, Row = 2, Type = CellType.String)]
        public string Title { get; set; }

        public IList<CarExcelModel> Cars { get; set; }
    }
}
