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

        public HistoryFlightsUserControl(IUserService userService)
        {
            InitializeComponent();
            var flightRepository = new FlightRepository();
            _flightService = new FlightService(flightRepository);           
            _userService = userService;
            LoadAllFlights();          
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
                var currentUser = mainWindow?.CurrentUser;

                if (currentUser.Role == "Worker")
                {
                    mainWindow.MainContent.Content = new AirplaneManagementControl(_userService);
                }
                else if (currentUser.Role == "User")
                {
                    mainWindow.MainContent.Content = new AirplaneUsersControl(_userService);
                }
            }
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new DashboardControl(_userService);
            }
        }
        private void LoadAllFlights()
        {
            var flights = _flightService.SearchFlights("", "", "", "", "", 0);
            FlightsDataGrid.ItemsSource = flights;
        }
    }
}
