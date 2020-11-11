using Estudiantes.Models;
using Estudiantes.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudiantes.Controllers
{
    [Authorize(Roles = Roles.SuperAdminRole)]
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();

            var userList = new List<UserRolesViewModel>();
            foreach (IdentityUser item in users)
            {
                var user = new UserRolesViewModel
                {
                    UserId = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Roles = new List<string>(await this.userManager.GetRolesAsync(item))
                };
                userList.Add(user);
            }

            return View(userList);
        }



        public async Task<IActionResult> AdminRoles(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con el ID = {userId} no se encuentra.";
                return NotFound();
            }

            ViewBag.userID = user.Id;
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (IdentityRole role in this.roleManager.Roles)
            {
                var usuarioRole = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await this.userManager.IsInRoleAsync(user, role.Name)
                };
                model.Add(usuarioRole);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdminRoles(string userId, List<ManageUserRolesViewModel> model)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"El usuario con el ID = {userId} no se encuentra.");
                return View();
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var result = await this.userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"No se pudo quitar los roles existentes del usuario.");
                return View();
            }

            result = await this.userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(x => x.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"No se pudo agregar los roles nuevos al usuario.");
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
