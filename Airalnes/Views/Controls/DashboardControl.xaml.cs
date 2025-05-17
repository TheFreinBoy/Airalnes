using Airalnes.Interfaces;
using Airalnes.Models;
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
using System.Windows.Shapes;


namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        private readonly IUserService _userService;
        public DashboardControl(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
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
    }
}
