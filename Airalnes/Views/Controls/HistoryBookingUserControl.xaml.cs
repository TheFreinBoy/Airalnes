using Airalnes.Interfaces;
using Airalnes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для HistoryBookingUserControl.xaml
    /// </summary>
    public partial class HistoryBookingUserControl : UserControl
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        public HistoryBookingUserControl()
        {
            InitializeComponent();
            _userService = App.UserService;
            _bookingService = App.BookingService;
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
            var mainWindow = Application.Current.MainWindow as MainWindow;
            var currentUser = mainWindow?.CurrentUser;
            var bookings = _bookingService.GetUserBookings(currentUser.Id);
            BookingHistroryDataGrid.ItemsSource = bookings;
        }
    }
}
