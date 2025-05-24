using Airalnes.Helpers;
using Airalnes.Interfaces;
using Airalnes.Models;
using Airalnes.Repositories;
using Airalnes.Services;
using Airalnes.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Airalnes.RepoInterfaces;
using Airalnes.ServiceInterfaces;

namespace Airalnes
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IDbConnectionFactory ConnectionFactory { get; private set; }
        public static IUserService UserService { get; private set; }
        public static IFlightService FlightService { get; private set; }
        public static IAirplaneService AirplaneService { get; private set; }
        public static IBookingService BookingService { get; private set; }
        public static IPaymentService PaymentService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string connectionString = "Data Source=mydatabase2.db;Version=3;";
            ConnectionFactory = new SqliteConnectionFactory(connectionString);

            var userRepository = new UserRepository(ConnectionFactory);
            UserService = new UserService(userRepository);

            var flightRepository = new FlightRepository(ConnectionFactory);
            FlightService = new FlightService(flightRepository);

            var airplaneRepository = new AirplaneRepository(ConnectionFactory);
            AirplaneService = new AirplaneService(airplaneRepository);
             
            var bookingRepository = new BookingRepostory(ConnectionFactory);
            BookingService = new BookingService(bookingRepository);

            var paymentRepository = new PaymentRepository(ConnectionFactory);
            PaymentService = new PaymentService(paymentRepository);
        }

    }

}
