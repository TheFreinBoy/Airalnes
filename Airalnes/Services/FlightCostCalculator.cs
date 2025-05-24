using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Services
{
    public static class FlightCostCalculator
    {
        private static readonly HashSet<string> UkraineAirports = new HashSet<string>
        {
            "KBP", "LWO", "ODS", "HRK"
        };

        private static readonly Dictionary<string, double> InternationalDestinations = new Dictionary<string, double>
        {
            { "LHR", 17000 }, // London
            { "CDG", 19000 }, // Paris
            { "JFK", 25000 }, // New York
            { "DXB", 21000 }, // Dubai
            { "FRA", 16000 }, // Frankfurt
            { "HND", 28000 }  // Tokyo
        };

        public static double Calculate(string from, string to)
        {
            bool isFromUkraine = UkraineAirports.Contains(from);
            bool isToUkraine = UkraineAirports.Contains(to);

            if (isFromUkraine && InternationalDestinations.TryGetValue(to, out double forwardPrice))
            {
                return forwardPrice;
            }

            if (isToUkraine && InternationalDestinations.TryGetValue(from, out double returnPrice))
            {
                return returnPrice;
            }

            return 15000;
        }
    }
}
