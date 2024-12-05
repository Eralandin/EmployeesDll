using Employees.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Services
{
    public static class DataTableAdapter
    {
        public static DataTable DataTableAdaptUsers(List<User> users)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Имя пользователя");
            table.Columns.Add("Роль");

            foreach (User user in users)
            {
                table.Rows.Add(user.Id,user.Username,user.Role);
            }
            return table;
        }
    }
}
