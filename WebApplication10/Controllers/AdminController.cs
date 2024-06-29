using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication10.DbContext;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager , ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult Create() => View();

        public async Task<IActionResult> UserList()
        {
            var users = _userManager.Users.ToList();
            var userListViewModel = new List<UserListViewModel>();

            foreach (var user in users)
            {
                userListViewModel.Add(new UserListViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                });
            }

            return View(userListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleName);
        }
        public IActionResult UsersAndRoles()
        {
            // Query to get all users with their roles
            var usersWithRoles = _dbContext.Users
                .Select(u => new UserWithRolesViewModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Roles = _userManager.GetRolesAsync(u).Result.ToList()
                })
                .ToList();

            return View(usersWithRoles);
        }


        [HttpGet]
        public async Task<IActionResult> Assign(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new AssignRoleViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Roles = _roleManager.Roles.ToList(),
                UserRoles = (List<string>)await _userManager.GetRolesAsync(user)
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Assign(AssignRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (model.SelectedRoles == null)
            {
                model.SelectedRoles = new List<string>();
            }

            try
            {
                // Add new roles to the user that they do not already have
                var rolesToAdd = model.SelectedRoles.Except(userRoles);
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    foreach (var error in addResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // Remove roles from the user that they should no longer have
                var rolesToRemove = userRoles.Except(model.SelectedRoles);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error assigning roles: {ex.Message}");
                return View(model);
            }
        }





    }
}
