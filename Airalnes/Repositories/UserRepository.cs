using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Helpers;
using Airalnes.Models;
using Airalnes.RepoInterfaces;

namespace Airalnes.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool IsUserExists(string column, string value)
        {
            var conn = _connectionFactory.CreateConnection();
            conn.Open();
            var query = $"SELECT COUNT(*) FROM users WHERE {column} = @Value";
            var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Value", value);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public void InsertUser(User user)
        {
            var conn = _connectionFactory.CreateConnection();
            conn.Open();
            var query = "INSERT INTO users (name, email, pass, rights) VALUES (@Name, @Email, @Pass, @Rights)";
            var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Pass", user.Password);
            cmd.Parameters.AddWithValue("@Rights", user.Role);
            cmd.ExecuteNonQuery();
        }

        public User GetUserByCredentials(string username, string password)
        {
            var conn = _connectionFactory.CreateConnection();
            conn.Open();
            var query = "SELECT id, name, rights FROM users WHERE name = @Username AND pass = @Password";
            var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Role = reader.GetString(2)
                };
            }

            return null;
        }
    }

}
