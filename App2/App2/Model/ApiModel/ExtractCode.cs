using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App2.Model.ApiModel
{
    public static class ExtractCode
    {
        public static List<string> ExtractCodes(string input)
        {
            // Usunięcie niepotrzebnych części ciągu
            string cleanedInput = input.Replace("twr_kod in (", "").Replace(")", "").Replace("'", "");

            // Podział ciągu na poszczególne kody
            var codes = cleanedInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Trim()) // Usunięcie białych znaków na początku i końcu każdego kodu
                                    .ToList();

            return codes;
        }
    }
    
}
