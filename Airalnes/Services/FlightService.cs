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
using Airalnes.RepoInterfaces;

namespace Airalnes.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _repository;

        public FlightService(IFlightRepository repository)
        {
            _repository = repository;
        }


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
            _repository.AddFlight(data);
        }
        public List<Flight> SearchFlights(string from, string to, string departure, string arrival, string flightClass, int passengers)
        {
            return _repository.SearchFlights(from, to, departure, arrival, flightClass, passengers);
        }
        public int GetNextAvailableFlightNumber()
        {
            return _repository.GetNextAvailableFlightNumber();
        }
        public void DeleteFlight(int flightId)
        {
           _repository.DeleteFlight(flightId);
        }

    }
}
