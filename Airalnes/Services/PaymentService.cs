using Airalnes.Models;
using Airalnes.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.RepoInterfaces;
using Airalnes.Repositories;

namespace Airalnes.Services
{
    internal class PaymentService: IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepositry)
        {
            _paymentRepository = paymentRepositry;
        }

        public void AddPayment(Payment payment)
        {
            _paymentRepository.AddPayment(payment);
        }
        
        public List<PaymentStatus> GetPaymentStatuses()
        {
            return _paymentRepository.GetPaymentStatuses();
        }
    }
}
