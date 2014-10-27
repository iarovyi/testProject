
namespace RestructuringTool.Excel.Submissions
{
    public interface IHeaderDateParser
    {
        bool CanParse(string input);

        ExcelAmountModel Parse(string headerString, double amount);
    }
}
