using Airalnes.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.RepoInterfaces;

namespace Airalnes.Database
{
    public class DatabaseSeeder : Repository
    {
        public DatabaseSeeder(IDbConnectionFactory connectionFactory) : base(connectionFactory) { }

        public void Seed()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                SeedPaymentStatus(connection);
                SeedAirplanesIfEmpty(connection);
                SeedAirportsIfEmpty(connection);
            }
        } 

        private void SeedPaymentStatus(SQLiteConnection connection)
        {
            string checkQuery = "SELECT COUNT(*) FROM PaymentStatus;";
            using (var command = new SQLiteCommand(checkQuery, connection))
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                {
                    string insertQuery = @"
                    INSERT INTO PaymentStatus (StatusName) VALUES ('Paid'), ('Unpaid');";
                    ExecuteQuery(insertQuery, connection);

                }
            }
        }

        private void SeedAirplanesIfEmpty(SQLiteConnection connection)
        {
            string checkQuery = "SELECT COUNT(*) FROM Airplanes;";
            using (var command = new SQLiteCommand(checkQuery, connection))
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                {
                    string insertQuery = @"
                    INSERT INTO Airplanes (name, capacity) VALUES
                        ('Boeing 737', 160),
                        ('Airbus A320', 150),
                        ('Boeing 777', 280),
                        ('Airbus A350', 314);";
                    ExecuteQuery(insertQuery, connection);

                }
            }
        }

        private void SeedAirportsIfEmpty(SQLiteConnection connection)
        {
            string checkQuery = "SELECT COUNT(*) FROM Airports;";
            using (var command = new SQLiteCommand(checkQuery, connection))
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                {
                    string insertQuery = @"
                    INSERT INTO Airports (name, city, country, iata_code) VALUES
                        ('Boryspil International Airport', 'Kyiv', 'Ukraine', 'KBP'),
                        ('Lviv Danylo Halytskyi International Airport', 'Lviv', 'Ukraine', 'LWO'),
                        ('Odesa International Airport', 'Odesa', 'Ukraine', 'ODS'),
                        ('Kharkiv International Airport', 'Kharkiv', 'Ukraine', 'HRK'),
                        ('Heathrow Airport', 'London', 'UK', 'LHR'),
                        ('Charles de Gaulle Airport', 'Paris', 'France', 'CDG'),
                        ('John F. Kennedy International Airport', 'New York', 'USA', 'JFK'),
                        ('Dubai International Airport', 'Dubai', 'UAE', 'DXB'),
                        ('Frankfurt am Main Airport', 'Frankfurt', 'Germany', 'FRA'),
                        ('Tokyo Haneda Airport', 'Tokyo', 'Japan', 'HND');";
                    ExecuteQuery(insertQuery, connection);

                }
            }
        }

        private void ExecuteQuery(string query, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
