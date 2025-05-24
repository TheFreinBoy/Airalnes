using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Airalnes;
using System.Windows.Media.Media3D;
using Airalnes.Models;
using Airalnes.Validators;
using Airalnes.Repositories;
using Airalnes.RepoInterfaces;

namespace Airalnes.Helpers
{
    public class DatabaseHelper:Repository
    {
        private string _databaseFile = "mydatabase2.db";

        public DatabaseHelper(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(_databaseFile))
            {
                SQLiteConnection.CreateFile(_databaseFile);
                using (var connection = GetConnection())
                {
                    connection.Open();
                    CreateTables(connection);
                    Console.WriteLine("xz");
                }
                Console.WriteLine("Створюємо БД");
            }
            Console.WriteLine("База даних вже є");
        }
        private void CreateTables(SQLiteConnection connection)
        {
            try
            {
                CreateUsersTable(connection);
                CreateAirplanesTable(connection);
                CreateFlightsTable(connection);
                CreateAirportsTable(connection);
                CreateBookingsTable(connection);
                CreatePayment(connection);
                CreatePaymentStatus(connection);
                             
                SeedAirplanesIfEmpty(connection);
                SeedAirportsIfEmpty(connection);
                SeedPaymentStatus(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при створенні таблиць: {ex.Message}");
            }
        }

        private void CreateUsersTable(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS Users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                email TEXT,
                pass TEXT NOT NULL,
                rights TEXT
            );";
            ExecuteQuery(query, connection);
        }
        private void CreateAirplanesTable(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS Airplanes (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                capacity INTEGER NOT NULL
            );";
            ExecuteQuery(query, connection);
        }
        private void CreateFlightsTable(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS Flights (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                from_location TEXT NOT NULL,
                to_location TEXT NOT NULL,
                departure TEXT NOT NULL,
                return_date TEXT,
                class TEXT NOT NULL,
                airplane_id INTEGER NOT NULL,
                flight_number TEXT UNIQUE,
                capacity INTEGER NOT NULL,
                time_DP TEXT NOT NULL,
                time_AR TEXT NOT NULL,
                FOREIGN KEY(airplane_id) REFERENCES Airplanes(id)
            );";
            ExecuteQuery(query, connection);
        }
        private void CreateAirportsTable(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS Airports (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                city TEXT NOT NULL,
                country TEXT NOT NULL,
                iata_code TEXT NOT NULL UNIQUE
            );";
            ExecuteQuery(query, connection);
        }

        private void CreateBookingsTable(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS Bookings (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                user_id INTEGER NOT NULL,
                flight_id INTEGER,
                booking_date INTEGER NOT NULL,
                name TEXT,
                surname TEXT,
                date_of_birth TEXT,
                payment_status_id INTEGER DEFAULT 2, -- 2 = Unpaid by default,
                FOREIGN KEY(user_id) REFERENCES Users(id),
                FOREIGN KEY(flight_id) REFERENCES Flights(id),
                FOREIGN KEY(payment_status_id) REFERENCES PaymentStatus(id)
            );";

            ExecuteQuery(query, connection);
        }

        private void CreatePayment(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE Payment (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                BookingId INTEGER NOT NULL,
                PaymentStatusId INTEGER NOT NULL,
                Amount REAL NOT NULL,
                PaymentDate DATETIME NOT NULL,
                FOREIGN KEY (UserId) REFERENCES Users(Id),
                FOREIGN KEY (BookingId) REFERENCES Booking(Id),
                FOREIGN KEY (PaymentStatusId) REFERENCES PaymentStatus(Id)
             );";
            ExecuteQuery(query, connection);
        }
        private void CreatePaymentStatus(SQLiteConnection connection)
        {
            string query = @"
            CREATE TABLE PaymentStatus(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StatusName TEXT NOT NULL
                );";
            ExecuteQuery(query, connection);
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
