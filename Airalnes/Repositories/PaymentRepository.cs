using Airalnes.Models;
using Airalnes.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Repositories
{
    internal class PaymentRepository: Repository,IPaymentRepository
    {
        public PaymentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) { }
        public void AddPayment(Payment payment)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "INSERT INTO Payment (UserId, BookingId, PaymentStatusId, Amount, PaymentDate) VALUES (@UserId, @BookingId, @StatusId, @Amount, @Date)",
                    connection);
                command.Parameters.AddWithValue("@UserId", payment.UserId);
                command.Parameters.AddWithValue("@BookingId", payment.BookingId);
                command.Parameters.AddWithValue("@StatusId", payment.PaymentStatusId);
                command.Parameters.AddWithValue("@Amount", payment.Amount);
                command.Parameters.AddWithValue("@Date", payment.PaymentDate.ToString("dd.MM.yyyy HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }

        public List<PaymentStatus> GetPaymentStatuses()
        {
            var statuses = new List<PaymentStatus>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM PaymentStatus", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        statuses.Add(new PaymentStatus
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            StatusName = reader["StatusName"].ToString()
                        });
                    }
                }
            }

            return statuses;
        }
    }
}
