using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Interfaces
{
    public interface IUserService
    {
        bool IsUserExists(string column, string value);
        bool RegisterUser(User user);
        User AuthenticateUser(string username, string password);
    }
}
