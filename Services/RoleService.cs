using Employees.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Services
{
    public class RoleService
    {
        private readonly string _connectionString;

        public RoleService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Role> GetRolesForUserAsync(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM tbRoles WHERE id_user = @UserId;";
                List<Role> roles = new List<Role>();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("UserId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Role
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                MenuItemId = reader.GetInt32(2),
                                AllowRead = reader.GetBoolean(3),
                                AllowWrite = reader.GetBoolean(4),
                                AllowEdit = reader.GetBoolean(5),
                                AllowDelete = reader.GetBoolean(6)
                            });
                        }
                        return roles;
                    }
                }
            }
            
        }

        public void AddRoleAsync(Role role)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO tbRoles (id_user, id_menuitem, allow_read, allow_write, allow_edit, allow_delete) 
                    VALUES (@UserId, @MenuItemId, @AllowRead, @AllowWrite, @AllowEdit, @AllowDelete);";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("UserId", role.UserId);
                    command.Parameters.AddWithValue("MenuItemId", role.MenuItemId);
                    command.Parameters.AddWithValue("AllowRead", role.AllowRead);
                    command.Parameters.AddWithValue("AllowWrite", role.AllowWrite);
                    command.Parameters.AddWithValue("AllowEdit", role.AllowEdit);
                    command.Parameters.AddWithValue("AllowDelete", role.AllowDelete);
                    command.ExecuteNonQuery();
                }
            }
            
        }

        public void UpdateRoleAsync(Role role)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    UPDATE tbRoles 
                    SET allow_read = @AllowRead, allow_write = @AllowWrite, allow_edit = @AllowEdit, allow_delete = @AllowDelete
                    WHERE id = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("AllowRead", role.AllowRead);
                    command.Parameters.AddWithValue("AllowWrite", role.AllowWrite);
                    command.Parameters.AddWithValue("AllowEdit", role.AllowEdit);
                    command.Parameters.AddWithValue("AllowDelete", role.AllowDelete);
                    command.Parameters.AddWithValue("Id", role.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRoleAsync(int roleId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM tbRoles WHERE id = @RoleId;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("RoleId", roleId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
