using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Airalnes.Views;
using Airalnes.Views.Controls;
using Airalnes.Services;
using Airalnes.Models;
using Airalnes.Interfaces;
using Airalnes.Validators;

namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private readonly IUserService _userService;
        private readonly LoginValidator _loginValidator;

        public LoginControl(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            _loginValidator = new LoginValidator(userService);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new RegistrationControl(_userService);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            UsernameTextBox.BorderBrush = string.IsNullOrEmpty(username) ? Brushes.Red : Brushes.White;
            PasswordBox.BorderBrush = string.IsNullOrEmpty(password) ? Brushes.Red : Brushes.White;

            var result = _loginValidator.Validate(username, password);

            if (!result.IsValid)
            {
                BlankError.Visibility = string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)
                    ? Visibility.Visible : Visibility.Collapsed;

                InvalidError.Visibility = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)
                    ? Visibility.Visible : Visibility.Collapsed;

                return;
            }

            BlankError.Visibility = Visibility.Collapsed;
            InvalidError.Visibility = Visibility.Collapsed;

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.CurrentUser = result.AuthenticatedUser;
                mainWindow.MainContent.Content = new DashboardControl(_userService);
            }
        }
    }
}
