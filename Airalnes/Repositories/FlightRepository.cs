using Airalnes.Helpers;
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
    public class FlightRepository : Repository, IFlightRepository
    {
        public FlightRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void AddFlight(Flight flight)
        {
            Console.WriteLine("Викликало");
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                INSERT INTO Flights (
                    from_location, to_location, departure, return_date, class,
                    airplane_id, flight_number, capacity, time_DP, time_AR
                ) VALUES (
                    @from, @to, @departure, @return, @class,
                    @airplane_id, @flight_number, @capacity, @timeDP, @timeAR
                );";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@from", flight.FromLocation);
                    cmd.Parameters.AddWithValue("@to", flight.ToLocation);
                    cmd.Parameters.AddWithValue("@departure", flight.Departure);
                    cmd.Parameters.AddWithValue("@return", flight.ReturnDate);
                    cmd.Parameters.AddWithValue("@class", flight.Class);
                    cmd.Parameters.AddWithValue("@airplane_id", flight.AirplaneId);
                    cmd.Parameters.AddWithValue("@flight_number", flight.FlightNumber);
                    cmd.Parameters.AddWithValue("@capacity", flight.Capacity);
                    cmd.Parameters.AddWithValue("@timeDP", flight.TimeDP);
                    cmd.Parameters.AddWithValue("@timeAR", flight.TimeAR);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Flight> SearchFlights(string from, string to, string departure, string returnDate, string flightClass, int passengers)
        {
            var flights = new List<Flight>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                SELECT f.id, f.from_location, f.to_location, f.departure, f.return_date,
                       f.class, a.name AS airplane_name, f.flight_number,
                       f.capacity, f.time_DP, f.time_AR
                FROM Flights f
                JOIN Airplanes a ON f.airplane_id = a.id
                WHERE (@from = '' OR f.from_location LIKE @from)
                  AND (@to = '' OR f.to_location LIKE @to)
                  AND (@departure = '' OR f.departure = @departure)
                  AND (@return_date = '' OR f.return_date = @return_date)
                  AND (@class = '' OR f.class = @class)
                  AND f.capacity >= @passengers;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@from", string.IsNullOrEmpty(from) ? "" : $"%{Utilits.ExtractAirportCode(from)}%");
                    command.Parameters.AddWithValue("@to", string.IsNullOrEmpty(to) ? "" : $"%{Utilits.ExtractAirportCode(to)}%");
                    command.Parameters.AddWithValue("@departure", string.IsNullOrEmpty(departure) ? "" : Utilits.ConvertFromIsoDate(departure));
                    command.Parameters.AddWithValue("@return_date", string.IsNullOrEmpty(returnDate) ? "" : Utilits.ConvertFromIsoDate(returnDate));
                    command.Parameters.AddWithValue("@class", flightClass ?? "");
                    command.Parameters.AddWithValue("@passengers", passengers);

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

        public int GetNextAvailableFlightNumber()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT MAX(CAST(flight_number AS INTEGER)) FROM Flights", connection);
                var result = command.ExecuteScalar();
                if (result != DBNull.Value && int.TryParse(result?.ToString(), out int lastNumber))
                {
                    return lastNumber + 1;
                }
                else
                {
                    return 1;
                }
            }
        }
        public void DeleteFlight(int flightId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Flights WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", flightId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
