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
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public int BookFlight(int userId, int flightId, string name, string surname, string dateOfBirth , int paymentStatusId)
        {
            return _bookingRepository.BookFlight(userId, flightId, name, surname, dateOfBirth, paymentStatusId);
        }


        public List<UserBookingInfo> GetUserBookings(int userId)
        {
            return _bookingRepository.GetUserBookings(userId);
        }
        public void UpdatePaymentStatus(int bookingId, int newStatusId)
        {
            _bookingRepository.UpdatePaymentStatus(bookingId, newStatusId);
        }
    }
}
