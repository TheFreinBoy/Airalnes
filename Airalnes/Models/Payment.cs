using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public int PaymentStatusId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
