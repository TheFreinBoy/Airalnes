using Airalnes.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Models;
using System.Data.SQLite;

namespace Airalnes.Repositories
{
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly string _connectionString = "Data Source=mydatabase2.db";
        public List<Airplane> GetAirplanes()
        {
            var airplanes = new List<Airplane>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, capacity FROM Airplanes;";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        airplanes.Add(new Airplane
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Capacity = reader.GetInt32(2)
                        });
                    }
                    Console.WriteLine("Da");
                }
            }

            return airplanes;
        }
        public List<Airport> GetAirports()
        {
            var airports = new List<Airport>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, city, country, iata_code FROM Airports;";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        airports.Add(new Airport
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            City = reader.GetString(2),
                            Country = reader.GetString(3),
                            IATACode = reader.GetString(4)
                        });
                    }
                    Console.WriteLine("Da");
                }
            }
            return airports;
        }
    }
}
