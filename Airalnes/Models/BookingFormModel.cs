using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Models
{
    public class BookingFormModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string FlightNumber { get; set; }
        public string CardNumber { get; set; }
        public string Cost { get; set; }
        public string CVV { get; set; }
        public string DateCard { get; set; }
    }
}
