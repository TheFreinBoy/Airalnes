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
using static MaterialDesignThemes.Wpf.Theme;
using Airalnes.Views;
using Airalnes.Models;
using Airalnes.Services;
using Airalnes.Interfaces;

namespace Airalnes.Validators
{
    public class RegistrationValidator
    {
        private readonly IUserService _userService;

        public RegistrationValidator(IUserService userService)
        {
            _userService = userService;
        }

        public (bool IsValid, string ErrorMessage) Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Role))
            {
                return (false, "Fields cannot be blank");
            }

            if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {              
                return (false, "Invalid email");
            }

            if (_userService.IsUserExists("name", user.Username))
            {
                return (false, "The username you have provided is already busy.");
            }

            if (_userService.IsUserExists("email", user.Email))
            {
                return (false, "The email you have provided is already busy.");
            }

            return (true, string.Empty);
        }
    }
}
