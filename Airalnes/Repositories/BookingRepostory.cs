using Airalnes.Models;
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
        public bool BookFlight(int userId, int flightId, string name, string surname, string dateOfBirth)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    string checkDuplicate = @"
                SELECT COUNT(*) FROM Bookings
                WHERE user_id = @userId AND flight_id = @flightId
                  AND name = @name AND surname = @surname AND date_of_birth = @dob";

                    using (var checkCmd = new SQLiteCommand(checkDuplicate, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@userId", userId);
                        checkCmd.Parameters.AddWithValue("@flightId", flightId);
                        checkCmd.Parameters.AddWithValue("@name", name);
                        checkCmd.Parameters.AddWithValue("@surname", surname);
                        checkCmd.Parameters.AddWithValue("@dob", dateOfBirth);

                        var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Such a reservation already exists.");
                            return false;
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
                            return false;
                        }
                    }


                    string insertBooking = @"
                INSERT INTO Bookings (user_id, flight_id, booking_date, name, surname, date_of_birth)
                VALUES (@userId, @flightId, @bookingDate, @name, @surname, @dob)";
                    using (var insertCmd = new SQLiteCommand(insertBooking, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@userId", userId);
                        insertCmd.Parameters.AddWithValue("@flightId", flightId);
                        insertCmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                        insertCmd.Parameters.AddWithValue("@name", name);
                        insertCmd.Parameters.AddWithValue("@surname", surname);
                        insertCmd.Parameters.AddWithValue("@dob", dateOfBirth);
                        insertCmd.ExecuteNonQuery();
                    }

                    string updateCapacity = "UPDATE Flights SET capacity = capacity - 1 WHERE id = @flightId";
                    using (var updateCmd = new SQLiteCommand(updateCapacity, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@flightId", flightId);
                        updateCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при бронюванні: {ex.Message}");
                return false;
            }
        }


        public List<Flight> GetUserBookings(int userId)
        {
            var flights = new List<Flight>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT f.id, f.from_location, f.to_location, f.departure, f.return_date,
                           f.class, a.name AS airplane_name, f.flight_number,
                           f.capacity, f.time_DP, f.time_AR
                    FROM Bookings b
                    JOIN Flights f ON b.flight_id = f.id
                    JOIN Airplanes a ON f.airplane_id = a.id
                    WHERE b.user_id = @userId;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flights.Add(new Flight
                            {
                                Id = reader.GetInt32(0),
                                FromLocation = reader.GetString(1),
                                ToLocation = reader.GetString(2),
                                Departure = reader.GetString(3),
                                ReturnDate = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Class = reader.GetString(5),
                                AirplaneName = reader.GetString(6),
                                FlightNumber = reader.GetString(7),
                                Capacity = reader.GetInt32(8),
                                TimeDP = reader.GetString(9),
                                TimeAR = reader.GetString(10)
                            });
                        }
                    }
                }
            }

            return flights;
        }
    }
}
