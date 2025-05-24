using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Interfaces
{
    public interface IBookingService
    {
        int BookFlight(int userId, int flightId, string name, string surname, string dateOfBirth, int paymentStatusId);
        List<UserBookingInfo> GetUserBookings(int userId);
        void UpdatePaymentStatus(int bookingId, int newStatusId);
    }
}
