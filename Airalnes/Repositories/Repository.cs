using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Helpers;
using Airalnes.Repositories;
using Airalnes.Interfaces;
using Airalnes.RepoInterfaces;

namespace Airalnes.Repositories
{
    public abstract class Repository
    {
        protected readonly IDbConnectionFactory _connectionFactory;

        protected Repository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected SQLiteConnection GetConnection()
        {
            return _connectionFactory.CreateConnection();
        }
    }
}
