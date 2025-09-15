using Airalnes.Interfaces;
using Airalnes.Models;
using Airalnes.Models.ValidationModels;
using Airalnes.ServiceInterfaces;
using Airalnes.Services;
using Airalnes.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly BookingValidator _bookingValidator;
        private readonly IPaymentService _paymentService;
        public BookingFlightUserControl(BookingContext context)
        {
            InitializeComponent();  
            _context = context;
            _userService = App.UserService;
            _bookingService = App.BookingService;
            _paymentService = App.PaymentService;
            _bookingValidator = new BookingValidator();
            DateOfBirthTextBox.DisplayDateEnd = DateTime.Today;
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
        private void Booking_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.BorderBrush = string.IsNullOrEmpty(NameTextBox.Text) ? Brushes.Red : Brushes.Black;
            DateOfBirthTextBox.BorderBrush = string.IsNullOrEmpty(DateOfBirthTextBox.Text) ? Brushes.Red : Brushes.Black;
            FlightNumberTextBox.BorderBrush = string.IsNullOrEmpty(FlightNumberTextBox.Text) ? Brushes.Red : Brushes.Black;
            SurnameTextBox.BorderBrush = string.IsNullOrEmpty(SurnameTextBox.Text) ? Brushes.Red : Brushes.Black;
            SexComboBox.BorderBrush = string.IsNullOrEmpty(SexComboBox.Text) ? Brushes.Red : Brushes.Black;           
            var form = new BookingFormModel
            {
                Name = NameTextBox.Text,
                Surname = SurnameTextBox.Text,
                DateOfBirth = DateOfBirthTextBox.Text,
                Sex = SexComboBox.Text,
                FlightNumber = FlightNumberTextBox.Text,              
            };

            var validator = new BookingValidator();
            var result = validator.Validate(form);

            if (!result.IsValid)
            {
                GlobalError.Text = result.ErrorMessage;
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
            int paymentStatusId = _paymentService.GetPaymentStatuses().First(s => s.StatusName == "Unpaid").Id;

            int bookingId = _bookingService.BookFlight(currentUser.Id, selectedFlight.Id, name, surname, dateOfBirth,paymentStatusId);


            if (bookingId > 0)
            {
                    MessageBox.Show("Booking successful!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
               
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = new PaymentUserControl(bookingId);
                }
            }                    
        }

    }
}
