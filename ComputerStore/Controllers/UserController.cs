using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ComputerStore.Models;
using ComputerStore.Models.ViewModels;

namespace ComputerStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            roleManager = roleMgr;
        }

        public ViewResult Index() => View(userManager.Users);

        [AllowAnonymous]
        public async Task<IActionResult> EditUser(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UpdateProfileViewModel viewModel = new UpdateProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Photo = user.Photo,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                AboutMe = user.AboutMe,
                SecurityStamp = user.SecurityStamp
            };
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UpdateProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByIdAsync(viewModel.Id);
                if (user != null)
                {
                    string role = userManager.GetRolesAsync(user).Result.FirstOrDefault();

                    user.FirstName = viewModel.FirstName;
                    user.LastName = viewModel.LastName;
                    user.Email = viewModel.Email;
                    user.PhoneNumber = viewModel.PhoneNumber;
                    if (!string.IsNullOrEmpty(viewModel.Photo))
                    {
                        user.Photo = viewModel.Photo;
                    }
                    user.Country = viewModel.Country;
                    user.City = viewModel.City;
                    user.Address = viewModel.Address;
                    user.AboutMe = viewModel.AboutMe;

                    await userManager.UpdateAsync(user);

                    if (!string.IsNullOrEmpty(role) && role != "Admin") {
                        return Redirect("/Home");
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            AppUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var roles = userManager.GetRolesAsync(user).Result;
            await userManager.RemoveFromRolesAsync(user, roles);
            await userManager.AddToRoleAsync(user, role);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));

        }
    }
}