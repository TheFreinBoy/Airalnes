using Airalnes.Interfaces;
using Airalnes.Models;
using Airalnes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для BookingFlightUserControl.xaml
    /// </summary>
    public partial class BookingFlightUserControl : UserControl
    {
        private BookingContext _context;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        public BookingFlightUserControl(BookingContext context)
        {
            InitializeComponent();  
            _context = context;
            _userService = App.UserService;
            _bookingService = App.BookingService;
            LoadFlightData();
        }
        private void LoadFlightData()
        {
            if (_context?.SelectedFlight != null)
            {
                FlightNumberTextBox.Text = _context.SelectedFlight.FlightNumber;
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
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            int cursorPosition = textBox.SelectionStart;
            string text = Regex.Replace(textBox.Text, @"\s+", "");

            if (text.Length > 16)
                text = text.Substring(0, 16);

            string formatted = string.Join(" ", Regex.Matches(text, @"\d{1,4}")
                                                   .Cast<Match>()
                                                   .Select(m => m.Value));

            if (textBox.Text != formatted)
            {
                textBox.Text = formatted;
                textBox.SelectionStart = Math.Min(cursorPosition + (formatted.Length - text.Length), formatted.Length);
            }
        }    
        private void DateCardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            string raw = textBox.Text.Replace("/", "");
            int selectionStart = textBox.SelectionStart;

            if (raw.Length > 4)
                raw = raw.Substring(0, 4);

            if (raw.Length >= 2)
            {
                string monthPart = raw.Substring(0, 2);
                if (!int.TryParse(monthPart, out int month) || month < 1 || month > 12)
                {
                    raw = raw.Substring(0, 1);
                }
            }

            string formatted = raw;
            if (raw.Length >= 3)
                formatted = raw.Insert(2, "/");

            if (textBox.Text != formatted)
            {
                textBox.Text = formatted;
                textBox.SelectionStart = Math.Min(formatted.Length, selectionStart);
            }
        }
        private void Booking_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.BorderBrush = string.IsNullOrEmpty(NameTextBox.Text) ? Brushes.Red : Brushes.Black;
            DateOfBirthTextBox.BorderBrush = string.IsNullOrEmpty(DateOfBirthTextBox.Text) ? Brushes.Red : Brushes.Black;
            FlightNumberTextBox.BorderBrush = string.IsNullOrEmpty(FlightNumberTextBox.Text) ? Brushes.Red : Brushes.Black;
            SurnameTextBox.BorderBrush = string.IsNullOrEmpty(SurnameTextBox.Text) ? Brushes.Red : Brushes.Black;
            SexComboBox.BorderBrush = string.IsNullOrEmpty(SexComboBox.Text) ? Brushes.Red : Brushes.Black;
            CardNumberTextBox.BorderBrush = string.IsNullOrEmpty(CardNumberTextBox.Text) ? Brushes.Red : Brushes.Black;
            CostTextBox.BorderBrush = string.IsNullOrEmpty(CostTextBox.Text) ? Brushes.Red : Brushes.Black;
            CVVTextBox.BorderBrush = string.IsNullOrEmpty(CVVTextBox.Text) ? Brushes.Red : Brushes.Black;
            DateCardTextBox.BorderBrush = string.IsNullOrEmpty(DateCardTextBox.Text) ? Brushes.Red : Brushes.Black;
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(DateOfBirthTextBox.Text) || string.IsNullOrEmpty(FlightNumberTextBox.Text) || string.IsNullOrEmpty(SexComboBox.Text)
                || string.IsNullOrEmpty(SurnameTextBox.Text) || string.IsNullOrEmpty(CardNumberTextBox.Text) || string.IsNullOrEmpty(CostTextBox.Text) || string.IsNullOrEmpty(CVVTextBox.Text)
                || string.IsNullOrEmpty(DateCardTextBox.Text))
            {
                GlobalError.Visibility = Visibility.Visible;
                return;
            }

            GlobalError.Visibility = Visibility.Collapsed;

            var mainWindow = Application.Current.MainWindow as MainWindow;
            var selectedFlight = _context.SelectedFlight;
            var currentUser = mainWindow?.CurrentUser;               
               
            string name = NameTextBox.Text.Trim();
            string surname = SurnameTextBox.Text.Trim();
            string dateOfBirth = DateOfBirthTextBox.Text.Trim();

            bool success = _bookingService.BookFlight(currentUser.Id, selectedFlight.Id, name, surname, dateOfBirth);

            if (success)
            {
                    MessageBox.Show("Бронювання успішне!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);                   
            }
            else
            {
                    MessageBox.Show("Не вдалося забронювати рейс. Можливо, немає доступних місць.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }          
        }

    }
}
