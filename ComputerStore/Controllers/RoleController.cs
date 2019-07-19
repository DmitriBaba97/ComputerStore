using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ComputerStore.Models;
using ComputerStore.Models.DbContexts;

namespace ComputerStore.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly AppIdentityDbContext identityDbContext;

        public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr,
            AppIdentityDbContext identityDbCntx)
        {
            roleManager = roleMgr;
            userManager = userMgr;
            identityDbContext = identityDbCntx;
        }

        public IActionResult Index() => View();

        public JsonResult GetRoles()
        {
            var allUserRoles = identityDbContext.UserRoles.ToList();
            var roles = from role in roleManager.Roles
                        let numOfUsers = allUserRoles.Count(ur => ur.RoleId == role.Id)
                        select new { role.Id, role.Name, numOfUsers };
            return Json(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            IdentityRole role = new IdentityRole { Name = name };
            await roleManager.CreateAsync(role);
            return Ok(new { role.Id, role.Name, numOfUsers = 0});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return BadRequest();
            }
            await roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}