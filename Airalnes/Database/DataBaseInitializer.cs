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

namespace Airalnes.Database
{
    public class DataBaseInitializer:Repository
    {
        private string _databaseFile = "mydatabase2.db";

        public DataBaseInitializer(IDbConnectionFactory connectionFactory) : base(connectionFactory)
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
                }
            }
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
        
        private void ExecuteQuery(string query, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
                                              
    }
    
}
