using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Models;
using Airalnes.Helpers;
using Airalnes.Interfaces;
using Airalnes.RepoInterfaces;

namespace Airalnes.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsUserExists(string column, string value)
        {
            return _userRepository.IsUserExists(column, value);
        }

        public bool RegisterUser(User user)
        {
            if (_userRepository.IsUserExists("email", user.Email))
                return false;

            _userRepository.InsertUser(user);
            return true;
        }

        public User AuthenticateUser(string username, string password)
        {
            return _userRepository.GetUserByCredentials(username, password);
        }
    }

}
