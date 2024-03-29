﻿using Guitaria.Core.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Guitaria.Web.Areas.Admin.AdminConstants;

namespace Guitaria.Web.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public AdminController(RoleManager<IdentityRole<Guid>> _roleManager)
        {
            roleManager = _roleManager;
        }

        public IActionResult ControlPage()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IdentityRole<Guid> identityRole = new IdentityRole<Guid>()
            {
                Name = model.RoleName
            };
            bool roleExists = await roleManager.RoleExistsAsync(identityRole.Name);
            if (roleExists)
            {
                ModelState.AddModelError("", "Role already exists.");
                return View(model);
            }
            IdentityResult result = await roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }
    }
}
