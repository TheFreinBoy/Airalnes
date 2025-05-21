using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Models;


namespace Airalnes.Interfaces
{     
    public interface IFlightService
    {
        void CreateFlight(Flight data);
        List<Flight> SearchFlights(string from, string to, string departure, string arrival, string flightClass, int passengers);
        int GetNextAvailableFlightNumber();
        void DeleteFlight(int flightId);
    }  
}
