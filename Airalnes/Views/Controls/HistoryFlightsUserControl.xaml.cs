using System;
using System.Collections.Generic;
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
using Airalnes.Helpers;
using Airalnes.Models;
using Airalnes.Interfaces;
using Airalnes.Services;
using Airalnes.Repositories;

namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для HistoryFlightsUserControl.xaml
    /// </summary>
    public partial class HistoryFlightsUserControl : UserControl
    {
        private readonly IFlightService _flightService;
        private readonly IUserService _userService;

        public HistoryFlightsUserControl()
        {
            InitializeComponent();
            _flightService = App.FlightService;
            _userService = App.UserService;
            LoadAllFlights();          
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void DeleteFlight_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Flight flightToDelete)
            {
                var result = MessageBox.Show($"Are you sure you want to delete?  {flightToDelete.FlightNumber}?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _flightService.DeleteFlight(flightToDelete.Id);
                    LoadAllFlights();
                }
            }
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
                var currentUser = mainWindow?.CurrentUser;

                if (currentUser.Role == "Worker")
                {
                    mainWindow.MainContent.Content = new AirplaneManagementControl();
                }
                else if (currentUser.Role == "User")
                {
                    mainWindow.MainContent.Content = new AirplaneUsersControl();
                }
            }
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new DashboardControl();
            }
        }
        private void LoadAllFlights()
        {
            var flights = _flightService.SearchFlights("", "", "", "", "", 0);
            FlightsDataGrid.ItemsSource = flights;
        }
    }
}
