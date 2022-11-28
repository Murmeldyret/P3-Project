using zenref.Core.Models;

namespace zenref.Core.Factories;

public class SpreadsheetFactory
{
    /// <summary>
    /// Creates a new reference with specified values.
    /// </summary>
    /// <returns></returns>
    public Spreadsheet CreateSpreadsheet()
    {
        return new Spreadsheet();
        {
            // Data here....
        };
    }
}