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
        public BookingFlightUserControl()
        {
            InitializeComponent();
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
                string userRights = mainWindow.CurrentUserRights;

                if (userRights == "Worker")
                {
                    mainWindow.MainContent.Content = new AirplaneManagementControl();
                }
                else if (userRights == "User")
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
        private void CVVTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
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
    }
}
