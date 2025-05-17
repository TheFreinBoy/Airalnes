using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Models;
using Airalnes.Helpers;
using Airalnes.Interfaces;
using System.Globalization;
using System.Windows;

namespace Airalnes.Services
{
    public class FlightService : IFlightService
    {
        private static readonly DatabaseHelper _dbHelper = new DatabaseHelper();

       
        public void CreateFlight(Flight data)
        {
            var format = "dd.MM.yyyy"; 
            var culture = CultureInfo.InvariantCulture;

            DateTime.TryParseExact(data.Departure, format, culture, DateTimeStyles.None, out var departureDate);
            DateTime.TryParseExact(data.ReturnDate, format, culture, DateTimeStyles.None, out var arrivalDate);
            
            if (departureDate >= arrivalDate)
            {
                MessageBox.Show("Departure date must be earlier than arrival date.");
            }
            _dbHelper.CreateFlight(
                from: data.FromLocation,
                to: data.ToLocation,
                departure: data.Departure,
                returnDate: data.ReturnDate,
                flightClass: data.Class,
                airplaneId: data.AirplaneId,
                flightNumber: data.FlightNumber,
                capacity: data.Capacity,
                timeDP: data.TimeDP,
                timeAR: data.TimeAR
            );
        }
        public List<Flight> SearchFlights(string from, string to, string departure, string arrival, string flightClass, int passengers)
        {
            return _dbHelper.SearchFlights(from, to, departure, arrival, flightClass, passengers);
        }
        public int GetNextAvailableFlightNumber()
        {
            return _dbHelper.GetNextAvailableFlightNumber();
        }
    }
}
