using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Models;

namespace Employees.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<User> GetUsersForCurrentUserAsync(User currentUser)
        {
            if (currentUser.Role != "Администратор")
            {
                throw new UnauthorizedAccessException("У Вас нет прав для посещения данной вкладки. Доступ запрещён.");
            }

            return GetUsersAsync();
        }

        public List<User> GetUsersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, username, password_hash, user_role FROM tbUsers;";
                List<User> users = new List<User>();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                Role = reader.GetString(3)
                            });
                        }
                    }
                }
                return users;
            }
        }

        public User GetUserByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, username, password_hash, user_role FROM tbUsers WHERE id = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                Role = reader.GetString(3)
                            };
                        }
                        else
                        {
                            throw new ArgumentException("Пользователь с таким ID не найден.");
                        }
                    }
                }
            }
            
        }

        public void AddUserAsync(User user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO tbUsers (username, password_hash, user_role) VALUES (@Username, @PasswordHash, @Role);";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Username", user.Username);
                    command.Parameters.AddWithValue("PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("Role", user.Role);
                    command.ExecuteNonQuery();
                }
            }
            
        }

        public void UpdateUserAsync(User user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE tbUsers SET username = @Username, password_hash = @PasswordHash, user_role = @Role WHERE id = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Username", user.Username);
                    command.Parameters.AddWithValue("PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("Role", user.Role);
                    command.Parameters.AddWithValue("Id", user.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUserAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM tbUsers WHERE id = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
