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
using Airalnes.Helpers;
using Airalnes.Interfaces;
using Airalnes.Models;
using Airalnes.Repositories;
using Airalnes.Services;
using Airalnes.Views.Controls;

namespace Airalnes.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; set; }     

        public MainWindow()
        {
            InitializeComponent();
            var connectionFactory = new SqliteConnectionFactory("Data Source=mydatabase2.db;Version=3;");
            var userRepo = new UserRepository(connectionFactory);
            IUserService userService = new UserService(userRepo);
            MainContent.Content = new LoginControl();

        }

        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
        }
        
    }
}
