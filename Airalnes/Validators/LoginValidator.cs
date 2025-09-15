using System;
using Airalnes.Models;
using Airalnes.Interfaces;

namespace Airalnes.Validators
{
    public class LoginValidator
    {
        private readonly IUserService _userService;

        public LoginValidator(IUserService userService)
        {
            _userService = userService;
        }

        public (bool IsValid, string ErrorMessage, User AuthenticatedUser) Validate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Username and password cannot be empty.", null);
            }

            var user = _userService.AuthenticateUser(username, password);

            if (user == null)
            {
                return (false, "Invalid username or password.", null);
            }

            return (true, string.Empty, user);
        }
    }
}
