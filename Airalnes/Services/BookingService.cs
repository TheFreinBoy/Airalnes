using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Airalnes.Models;
using Airalnes.Helpers;
using System.Windows;
using Airalnes.Interfaces;
using Airalnes.RepoInterfaces;
using System.Xml.Linq;

namespace Airalnes.Services
{
    public class BookingService : IBookingService
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public bool BookFlight(int userId, int flightId, string name, string surname, string dateOfBirth)
        {
            return _bookingRepository.BookFlight(userId, flightId, name, surname, dateOfBirth);
        }


        public List<Flight> GetUserBookings(int userId)
        {
            return _bookingRepository.GetUserBookings(userId);
        }
    }
}
