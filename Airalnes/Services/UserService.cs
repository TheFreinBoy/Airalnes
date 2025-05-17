using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Models;
using Airalnes.Helpers;
using Airalnes.Interfaces;

namespace Airalnes.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseHelper _dbHelper = new DatabaseHelper();

        public bool IsUserExists(string column, string value)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = $"SELECT COUNT(*) FROM users WHERE {column} = @Value";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Value", value);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public bool RegisterUser(User user)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string insertQuery = "INSERT INTO users (name, email, pass, rights) VALUES (@Username, @Email, @Password, @Role)";
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public User AuthenticateUser(string username, string password)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT id, name, rights FROM users WHERE name = @Username AND pass = @Password";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Role = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
