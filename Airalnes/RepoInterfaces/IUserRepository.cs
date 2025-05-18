using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.RepoInterfaces
{
    public interface IUserRepository
    {
        bool IsUserExists(string column, string value);
        void InsertUser(User user);
        User GetUserByCredentials(string username, string password);
    }

}
