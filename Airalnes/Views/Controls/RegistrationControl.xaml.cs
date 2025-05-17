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
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using Airalnes.Views;
using Airalnes.Models;
using Airalnes.Services;
using Airalnes.Interfaces;
using Airalnes.Validators;

namespace Airalnes.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для RegistrationControl.xaml
    /// </summary>
    public partial class RegistrationControl : UserControl
    {
        private readonly IUserService _userService;
        private readonly RegistrationValidator _validator;
        public RegistrationControl(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            _validator = new RegistrationValidator(userService);
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new LoginControl(_userService);
            }
        }       
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.BorderBrush = string.IsNullOrWhiteSpace(UsernameTextBox.Text) ? Brushes.Red : Brushes.White;
            EmailTextBox.BorderBrush = string.IsNullOrWhiteSpace(EmailTextBox.Text) ? Brushes.Red : Brushes.White;
            PasswordBox.BorderBrush = string.IsNullOrWhiteSpace(PasswordBox.Password) ? Brushes.Red : Brushes.White;
            RoleComboBox.BorderBrush = string.IsNullOrWhiteSpace(RoleComboBox.Text) ? Brushes.Red : Brushes.White;
            var user = new User
            {
                Username = UsernameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                Role = RoleComboBox.Text
            };

            var result = _validator.Validate(user);

            if (!result.IsValid)
            {
                GlobalError.Text = result.ErrorMessage;
                GlobalError.Visibility = Visibility.Visible;
                return;
            }                     
                bool isRegistered = _userService.RegisterUser(user);
            if (isRegistered)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContent.Content = new LoginControl(_userService);
            }
            else
            {
                RegistrationError.Visibility = Visibility.Visible;
            }
        }


    }
    

}
