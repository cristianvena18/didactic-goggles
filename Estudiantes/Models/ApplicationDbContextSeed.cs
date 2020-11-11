using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Models
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdminRole));
            await roleManager.CreateAsync(new IdentityRole(Roles.AdminRole));
            await roleManager.CreateAsync(new IdentityRole(Roles.ModeratorRole));
            await roleManager.CreateAsync(new IdentityRole(Roles.StudentRole));
        }

        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager)
        {
            var userAdmin = userManager.Users.Where(x => x.Email == Roles.MailSuperAdmin).FirstOrDefault();
            if (userAdmin != null) return;

            userAdmin = new IdentityUser
            {
                UserName = Roles.MailSuperAdmin,
                Email = Roles.MailSuperAdmin,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            await userManager.CreateAsync(userAdmin, "Clave1");
            await userManager.AddToRoleAsync(userAdmin, Roles.SuperAdminRole);

        }
    }
}
