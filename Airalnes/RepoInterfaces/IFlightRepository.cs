using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.RepoInterfaces
{
    public interface IFlightRepository
    {
        void AddFlight(Flight flight);
        List<Flight> SearchFlights(string from, string to, string departure, string arrival, string flightClass, int passengers);
        int GetNextAvailableFlightNumber();
        void DeleteFlight(int flightId);
    }

}
