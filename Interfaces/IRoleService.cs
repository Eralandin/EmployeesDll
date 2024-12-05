using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels;

namespace Employees.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesForUserAsync(int userId);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
    }
}
