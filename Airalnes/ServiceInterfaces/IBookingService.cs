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
        bool BookFlight(int userId, int flightId, string name, string surname, string dateOfBirth);
        List<Flight> GetUserBookings(int userId);
    }
}
