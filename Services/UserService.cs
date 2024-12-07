using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Employees.Services
{
    public class UserService
    {
        private readonly string _connectionString;
        private User _currentUser;
        private Form? mainForm;

        public UserService(string connectionString, User currentUser, Form? mainForm)
        {
            _connectionString = connectionString;
            _currentUser = currentUser;
            if (mainForm != null)
            {
                this.mainForm = mainForm;
            }
        }
        public List<User> GetUsersAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, username, password_hash, user_role, isadmin FROM tbUsers WHERE id != @Id;";
                List<User> users = new List<User>();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id",_currentUser.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                Role = reader.GetString(3),
                                IsAdmin = reader.GetBoolean(4)
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
        public void ChangeUserData()
        {
            AddForm addForm = new AddForm();
            addForm.SetConnectionString(_connectionString);
            addForm._currentUserId = _currentUser.Id;
            if (_currentUser.IsAdmin)
            {
                addForm.AdminCheck.Visible = true;
            }
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM tbUsers WHERE id = @SelectedId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("SelectedId", _currentUser.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            addForm.UsernameTextBox.Text = reader.GetString(1);
                            addForm.RoleTextBox.Text = reader.GetString(3);
                            addForm.AdminCheck.Checked = reader.GetBoolean(4);
                            addForm.PasswordCheckTextBox.Enabled = false;
                            addForm.PasswordTextBox.Enabled = false;
                            addForm.PasswordCheckLabel.Enabled = false;
                            addForm.PasswordLabel.Enabled = false;
                        }
                    }
                }
            }
            var userRoles = addForm.GetUserRoles((long)_currentUser.Id);
            addForm.MarkTreeViewNodes(addForm.TreeView, userRoles);
            addForm.CreateBtn.Click += addForm.UpdateUser;
            addForm.ShowDialog();
            if (addForm.DialogResult == DialogResult.OK)
            {
                addForm.Message("Произведено изменение данных пользователя. Пройдите авторизацию повторно.");

                mainForm.Close();
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
