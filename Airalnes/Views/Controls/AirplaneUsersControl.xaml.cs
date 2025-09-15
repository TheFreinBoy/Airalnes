using System;
using System.Collections.Generic;
using System.Data;
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
using Airalnes.Views;
using Airalnes.Views.Controls;
using Airalnes.Models;
using Airalnes.Helpers;
using Airalnes.Services;
using System.Runtime.Remoting.Contexts;
using Airalnes.Interfaces;
using Airalnes.Repositories;

namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для AirplaneUsersControl.xaml
    /// </summary>
    public partial class AirplaneUsersControl : UserControl
    {
        private readonly IAirplaneService _airplaneService;
        private readonly IFlightService _flightService;
        private readonly IUserService _userService;
        private BookingContext _context;
        public AirplaneUsersControl()
        {
            InitializeComponent();

            _airplaneService = App.AirplaneService;
            _flightService = App.FlightService;           
            _context = new BookingContext();          
            _userService = App.UserService;
            LoadAirports();
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
                mainWindow.MainContent.Content = new LoginControl();
            }

        }
        private void Airplane_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new DashboardControl();
            }
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var currentUser = mainWindow?.CurrentUser;

                if (currentUser.Role == "Worker")
                {
                    mainWindow.MainContent.Content = new HistoryFlightsUserControl();
                }
                else if (currentUser.Role == "User")
                {
                    mainWindow.MainContent.Content = new HistoryBookingUserControl();
                }
            }
        }
        private void LoadAirports()
        {
            var airports = _airplaneService.GetAllAirports();
            FromTextBox.ItemsSource = airports;
            ToTextBox.ItemsSource = airports;
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            FromTextBox.BorderBrush = string.IsNullOrEmpty(FromTextBox.Text) ? Brushes.Red : Brushes.Black;
            ToTextBox.BorderBrush = string.IsNullOrEmpty(ToTextBox.Text) ? Brushes.Red : Brushes.Black;
            PassengersComboBox.BorderBrush = string.IsNullOrEmpty(PassengersComboBox.Text) ? Brushes.Red : Brushes.Black;
            DepartureTextBox.BorderBrush = DepartureTextBox.SelectedDate == null ? Brushes.Red : Brushes.Black;
            ArrivalTextBox.BorderBrush = ArrivalTextBox.SelectedDate == null ? Brushes.Red : Brushes.Black;
            ClassComboBox.BorderBrush = string.IsNullOrEmpty(ClassComboBox.Text) ? Brushes.Red : Brushes.Black;
            if (string.IsNullOrEmpty(FromTextBox.Text) || string.IsNullOrEmpty(ToTextBox.Text) || DepartureTextBox.SelectedDate == null || 
                ArrivalTextBox.SelectedDate == null || string.IsNullOrEmpty(PassengersComboBox.Text) || string.IsNullOrEmpty(ClassComboBox.Text))
            {
                FieldsError.Visibility = Visibility.Visible;
                return;
            }           
            
            FieldsError.Visibility = Visibility.Collapsed;

                string from = FromTextBox.Text;
                string to = ToTextBox.Text;
                string departure = DepartureTextBox.SelectedDate?.ToString("yyyy-MM-dd") ?? "";
                string arrival = ArrivalTextBox.SelectedDate?.ToString("yyyy-MM-dd") ?? "";
                string flightClass = (ClassComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            int passengers = int.Parse(PassengersComboBox.Text);

            var results = _flightService.SearchFlights(from, to, departure, arrival, flightClass, passengers);
            FlightsDataGrid.ItemsSource = results;
            
        }
        private void BookFlight_Click(object sender, RoutedEventArgs e)
        {           
            var selectedFlight = FlightsDataGrid.SelectedItem as Flight;           
            if (selectedFlight == null)
            {
                BookError.Visibility = Visibility.Visible;
                return;
            }
            if (_context == null)
            {            
                MessageBox.Show("Контекст не ініцільований!");
                return;
            }
            BookError.Visibility = Visibility.Collapsed;
            _context.SelectedFlight = selectedFlight;

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new BookingFlightUserControl(_context);
            }
        }

    }
}
