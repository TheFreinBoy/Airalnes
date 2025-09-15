using Airalnes.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Models
{
    public class UserBookingInfo
    {
        public int BookingId { get; set; }
        public Flight Flight { get; set; }
        public string PaymentStatus { get; set; }
        public PaymentStatusEnum PaymentStatusId { get; set; }
    }
}
