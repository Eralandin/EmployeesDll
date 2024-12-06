using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace Employees.Services
{
    public class UserService
    {
        private readonly string _connectionString;
        private User _currentUser;

        public UserService(string connectionString, User currentUser)
        {
            _connectionString = connectionString;
            _currentUser = currentUser;
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
        public void ChangePassword()
        {
            ChangePasswordForm changeForm = new ChangePasswordForm();
            string newPassword;
            if (changeForm.ShowDialog() == DialogResult.OK)
            {
                newPassword = changeForm.PasswordTextBox.Text;
                changeForm.Close();
            }
            else
            {
                MessageBox.Show("Операция отменена пользователем");
                return;
            }
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "UPDATE tbUsers SET password_hash = @PasswordHash WHERE id = @Id;";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("PasswordHash", User.HashPassword(newPassword));
                        command.Parameters.AddWithValue("Id", _currentUser.Id);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Смена пароля прошла успешно!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
