using Airalnes.Helpers;
using Airalnes.Interfaces;
using Airalnes.Models;
using Airalnes.Models.ValidationModels;
using Airalnes.ServiceInterfaces;
using Airalnes.Services;
using Airalnes.Validators;
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
    /// Логика взаимодействия для PaymentUserControl.xaml
    /// </summary>
    public partial class PaymentUserControl : UserControl
    {
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private int _bookingId;
        public PaymentUserControl(int bookingId)
        {
            InitializeComponent();
            _bookingId = bookingId;
            _userService = App.UserService;
            _paymentService = App.PaymentService;
            _bookingService = App.BookingService;
            Cost();
        }
        private void Cost()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            var currentUser = mainWindow?.CurrentUser;
          
            var bookings = _bookingService.GetUserBookings(currentUser.Id);

            var booking = bookings.FirstOrDefault(b => b.BookingId == _bookingId);
            var flight = booking.Flight;
            double cost = FlightCostCalculator.Calculate(flight.FromLocation, flight.ToLocation);
            CostTextBox.Text = cost.ToString("F2");        
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
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !InputFormatter.IsDigitInput(e.Text);
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputFormatter.FormatCardNumber(sender as TextBox);
        }

        private void DateCardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputFormatter.FormatCardDate(sender as TextBox);
        }

        private void PayButton_Click(object sender, RoutedEventArgs e) 
        {
            CardNumberTextBox.BorderBrush = string.IsNullOrEmpty(CardNumberTextBox.Text) ? Brushes.Red : Brushes.Black;
            CardNumberTextBox.BorderBrush = string.IsNullOrEmpty(CardNumberTextBox.Text) ? Brushes.Red : Brushes.Black;
            CVVTextBox.BorderBrush = string.IsNullOrEmpty(CVVTextBox.Text) ? Brushes.Red : Brushes.Black;
            DateCardTextBox.BorderBrush = string.IsNullOrEmpty(DateCardTextBox.Text) ? Brushes.Red : Brushes.Black;
            var form = new PaymentValidationModel
            {
                CardNumber = CardNumberTextBox.Text,
                CVV = CVVTextBox.Text,
                DateCard = DateCardTextBox.Text,
                
            };
            var validator = new PaymentValidator();
            var result = validator.Validate(form);

            if (!result.IsValid)
            {
                GlobalError.Text = result.ErrorMessage;
                GlobalError.Visibility = Visibility.Visible;
                return;
            }

            GlobalError.Visibility = Visibility.Collapsed;

            var mainWindow = Application.Current.MainWindow as MainWindow;
            var currentUser = mainWindow?.CurrentUser;
            double.TryParse(CostTextBox.Text, out double amount);            
            _paymentService.AddPayment(new Payment
            {
                UserId = currentUser.Id,
                BookingId = _bookingId,
                PaymentStatusId = 1,
                Amount = amount,
                PaymentDate = DateTime.Now
            });
            MessageBox.Show("Successful");
            _bookingService.UpdatePaymentStatus(_bookingId, 1);
        }
    }
}
