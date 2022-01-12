using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FoodDelivery.Pages.Settings
{
    [Authorize(Policy = PolicyName.IsAdmin)]
    public class BlockUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlockUserModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Username")]
            [BindProperty]
            public string UserName { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userName = Input.UserName ?? "";

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                StatusMessage = "Error: no user exists with the defined username.";
                return RedirectToPage();
            }

            var userClaims =
                from uc in _context.UserClaims
                where uc.UserId == user.Id
                select uc.ClaimValue;

            if (userClaims.Contains(RoleName.Admin))
            {
                StatusMessage = "Error: " + userName + " is an admin.";
                return RedirectToPage();
            }

            var userLockEnd = await _userManager.GetLockoutEndDateAsync(user);

            if (userLockEnd != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                StatusMessage = "Success: " + userName + " is now unlocked.";
                return RedirectToPage();
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(200));
                StatusMessage = "Success: " + userName + " is now blocked.";
                return RedirectToPage();
            }
        }
    }
}
