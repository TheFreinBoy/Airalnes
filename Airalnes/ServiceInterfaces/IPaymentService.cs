using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.ServiceInterfaces
{
    public interface IPaymentService
    {
        void AddPayment(Payment payment);
        List<PaymentStatus> GetPaymentStatuses();
    }
}
