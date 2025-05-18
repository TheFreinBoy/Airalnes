using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Helpers
{
    public class Utilits
    {
        public static string ExtractAirportCode(string input)
        {
            if (input.Contains("(") && input.Contains(")"))
            {
                int start = input.IndexOf('(') + 1;
                int end = input.IndexOf(')');
                return input.Substring(start, end - start);
            }
            return input;
        }

        public static string ConvertFromIsoDate(string isoDateStr)
        {
            if (DateTime.TryParse(isoDateStr, out DateTime date))
            {
                return date.ToString("dd.MM.yyyy");
            }
            return isoDateStr;
        }
    }
}
