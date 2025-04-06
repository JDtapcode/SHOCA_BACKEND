using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class InitialSeeding
    {
        private static readonly string[] RoleList =
            [Enums.Role.Admin.ToString(), Enums.Role.Customer.ToString(), Enums.Role.Staff.ToString()];

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (string role in RoleList)
            {
                Role? existedRole = await roleManager.FindByNameAsync(role);
                if (existedRole == null)
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }
        }
    }
}
