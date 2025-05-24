using Airalnes.Models;
using Airalnes.Models.Enum;
using Airalnes.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airalnes.Repositories
{
    internal class BookingRepostory : Repository, IBookingRepository
    {
        public BookingRepostory(IDbConnectionFactory connectionFactory) : base(connectionFactory) { }
        public int BookFlight(int userId, int flightId, string name, string surname, string dateOfBirth, int paymentStatusId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    string checkDuplicate = @"
                SELECT COUNT(*) FROM Bookings
                WHERE user_id = @userId AND flight_id = @flightId
                  AND name = @name AND surname = @surname AND date_of_birth = @dob AND payment_status_id = @paymentstatusId";

                    using (var checkCmd = new SQLiteCommand(checkDuplicate, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@userId", userId);
                        checkCmd.Parameters.AddWithValue("@flightId", flightId);
                        checkCmd.Parameters.AddWithValue("@name", name);
                        checkCmd.Parameters.AddWithValue("@surname", surname);
                        checkCmd.Parameters.AddWithValue("@dob", dateOfBirth);
                        checkCmd.Parameters.AddWithValue("@paymentstatusId", paymentStatusId);

                        var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Such a reservation already exists.");
                            return 0;
                        }
                    }

                    string capacityQuery = "SELECT capacity FROM Flights WHERE id = @flightId";
                    using (var checkCapacity = new SQLiteCommand(capacityQuery, connection))
                    {
                        checkCapacity.Parameters.AddWithValue("@flightId", flightId);
                        int capacity = Convert.ToInt32(checkCapacity.ExecuteScalar());
                        if (capacity <= 0)
                        {
                            MessageBox.Show("There are no available seats.");
                            return 0;
                        }
                    }

                    
                    string insertBooking = @"
                INSERT INTO Bookings (user_id, flight_id, booking_date, name, surname, date_of_birth, payment_status_id)
                VALUES (@userId, @flightId, @bookingDate, @name, @surname, @dob, @paymentstatusId)";
                    using (var insertCmd = new SQLiteCommand(insertBooking, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@userId", userId);
                        insertCmd.Parameters.AddWithValue("@flightId", flightId);
                        insertCmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                        insertCmd.Parameters.AddWithValue("@name", name);
                        insertCmd.Parameters.AddWithValue("@surname", surname);
                        insertCmd.Parameters.AddWithValue("@dob", dateOfBirth);
                        insertCmd.Parameters.AddWithValue("@paymentstatusId", paymentStatusId);

                        insertCmd.ExecuteNonQuery();
                    }
                    long bookingId;
                    using (var getIdCmd = new SQLiteCommand("SELECT last_insert_rowid();", connection))
                    {
                        bookingId = (long)getIdCmd.ExecuteScalar();
                    }

                    string updateCapacity = "UPDATE Flights SET capacity = capacity - 1 WHERE id = @flightId";
                    using (var updateCmd = new SQLiteCommand(updateCapacity, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@flightId", flightId);
                        updateCmd.ExecuteNonQuery();
                    }

                    return (int)bookingId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при бронюванні: {ex.Message}");
                return 0;
            }
        }


        public List<UserBookingInfo> GetUserBookings(int userId)
        {
            var bookings = new List<UserBookingInfo>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"
            SELECT 
                b.id AS booking_id,
                f.id, f.from_location, f.to_location, f.departure, f.return_date,
                f.class, a.name AS airplane_name, f.flight_number,
                f.capacity, f.time_DP, f.time_AR,
                ps.StatusName,
                b.payment_status_id
            FROM Bookings b
            JOIN Flights f ON b.flight_id = f.id
            JOIN Airplanes a ON f.airplane_id = a.id
            JOIN PaymentStatus ps ON b.payment_status_id = ps.id
            WHERE b.user_id = @userId;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bookingId = reader.GetInt32(0);        
                            var flight = new Flight
                            {
                                Id = reader.GetInt32(1),
                                FromLocation = reader.GetString(2),
                                ToLocation = reader.GetString(3),
                                Departure = reader.GetString(4),
                                ReturnDate = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Class = reader.GetString(6),
                                AirplaneName = reader.GetString(7),
                                FlightNumber = reader.GetString(8),
                                Capacity = reader.GetInt32(9),
                                TimeDP = reader.GetString(10),
                                TimeAR = reader.GetString(11)
                            };

                            string paymentStatus = reader.GetString(12);
                            int paymentStatusId = reader.GetInt32(13);

                            bookings.Add(new UserBookingInfo
                            {
                                BookingId = bookingId,
                                Flight = flight,
                                PaymentStatus = paymentStatus,
                                PaymentStatusId = (PaymentStatusEnum)paymentStatusId
                            });
                        }

                    }
                }
            }

            return bookings;
        }

        public void UpdatePaymentStatus(int bookingId, int newStatusId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string updateQuery = "UPDATE Bookings SET payment_status_id = @statusId WHERE id = @bookingId";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@statusId", newStatusId);
                    command.Parameters.AddWithValue("@bookingId", bookingId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
