using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;
using Airalnes.Views;
using Airalnes.Views.Controls;
using Airalnes.Models;
using Airalnes.Services;
using Airalnes.Interfaces;
using Airalnes.Repositories;

namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для AirplaneManagementControl.xaml
    /// </summary>
    public partial class AirplaneManagementControl : UserControl
    {

        private readonly IAirplaneService airplaneService;
        private readonly IFlightService _flightService;
        private readonly IUserService _userService;
        public AirplaneManagementControl(IUserService userService)
        {
            InitializeComponent();            
            var airplaneRepository = new AirplaneRepository();
            airplaneService = new AirplaneService(airplaneRepository);
            var flightRepository = new FlightRepository();
            _flightService = new FlightService(flightRepository);
            var nextFlightNumber = _flightService.GetNextAvailableFlightNumber();
            _userService = userService;
            FlightNumberTextBox.Text = nextFlightNumber.ToString();
            LoadAirplanes();
            LoadAirports();
        }
        
        private void AirplaneManagementControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNextFlightNumber();
        }

        private void LoadNextFlightNumber()
        {
            try
            {
                int nextNumber = _flightService.GetNextAvailableFlightNumber();
                FlightNumberTextBox.Text = nextNumber.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading flight number: " + ex.Message);
            }
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            UserPopup.IsOpen = !UserPopup.IsOpen;
        }     
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new LoginControl(_userService);
            }

        }
        private void Airplane_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new DashboardControl(_userService);
            }

        }

        private void LoadAirplanes()
        {
            AirplaneComboBox.ItemsSource = airplaneService.GetAllAirplanes();
        }

        private void LoadAirports()
        {
            var airports = airplaneService.GetAllAirports();
            FromTextBox.ItemsSource = airports;
            ToTextBox.ItemsSource = airports;
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var currentUser = mainWindow?.CurrentUser;

                if (currentUser.Role == "Worker")
                {
                    mainWindow.MainContent.Content = new HistoryFlightsUserControl(_userService);
                }
                else if (currentUser.Role == "User")
                {
                    mainWindow.MainContent.Content = new AirplaneUsersControl(_userService);
                }
            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var fromAirport = FromTextBox.SelectedItem as Airport;
            var toAirport = ToTextBox.SelectedItem as Airport;
            var selectedAirplane = AirplaneComboBox.SelectedItem as Airplane;
            FromTextBox.BorderBrush = string.IsNullOrEmpty(FromTextBox.Text) ? Brushes.Red : Brushes.Black;
            ToTextBox.BorderBrush = string.IsNullOrEmpty(ToTextBox.Text) ? Brushes.Red : Brushes.Black;           
            DepartureTextBox.BorderBrush = string.IsNullOrEmpty(DepartureTextBox.Text) ? Brushes.Red : Brushes.Black;
            ArrivalTextBox.BorderBrush = string.IsNullOrEmpty(ArrivalTextBox.Text) ? Brushes.Red : Brushes.Black;
            ClassComboBox.BorderBrush = string.IsNullOrEmpty(ClassComboBox.Text) ? Brushes.Red : Brushes.Black;
            AirplaneComboBox.BorderBrush = string.IsNullOrEmpty(AirplaneComboBox.Text) ? Brushes.Red : Brushes.Black;
            FlightNumberTextBox.BorderBrush = string.IsNullOrEmpty(FlightNumberTextBox.Text) ? Brushes.Red : Brushes.Black;
            DepartureTimePicker.BorderBrush = string.IsNullOrEmpty(DepartureTimePicker.Text) ? Brushes.Red : Brushes.Black;
            ArrivalTimePicker.BorderBrush = string.IsNullOrEmpty(ArrivalTimePicker.Text) ? Brushes.Red : Brushes.Black;
            if (string.IsNullOrEmpty(FromTextBox.Text) || string.IsNullOrEmpty(ToTextBox.Text) || string.IsNullOrEmpty(DepartureTextBox.Text) || string.IsNullOrEmpty(ArrivalTextBox.Text) 
                || string.IsNullOrEmpty(ClassComboBox.Text) || string.IsNullOrEmpty(AirplaneComboBox.Text) || string.IsNullOrEmpty(FlightNumberTextBox.Text) || string.IsNullOrEmpty(DepartureTimePicker.Text) 
                || string.IsNullOrEmpty(ArrivalTimePicker.Text))
            {
                GlobalError.Visibility = Visibility.Visible;
                return;
            }
            GlobalError.Visibility = Visibility.Collapsed;

            var formData = new Flight
            {
                FromLocation = fromAirport.IATACode,
                ToLocation = toAirport.IATACode,
                Departure = DepartureTextBox.Text,
                ReturnDate = ArrivalTextBox.Text,
                Class = ClassComboBox.Text,
                AirplaneId = selectedAirplane.Id,
                Capacity = selectedAirplane.Capacity,
                FlightNumber = FlightNumberTextBox.Text,
                TimeDP = DepartureTimePicker.Text,
                TimeAR = ArrivalTimePicker.Text
            };

            try
            {
                _flightService.CreateFlight(formData);
            }
            catch
            {
                GlobalError.Visibility = Visibility.Visible;
            }
        }
    }
    }


