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

namespace FoodDelivery.Pages.Admin
{
    [Authorize(Policy = PolicyName.IsAdmin)]
    public class BlockModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlockModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            [Required]
            public string UserName { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByNameAsync(Input.UserName ?? "");

            if (user == null)
            {
                StatusMessage = "Error: no user exists with the defined username.";
                return RedirectToPage();
            }

            var userClaims =
                from claim in _context.UserClaims
                where claim.UserId == user.Id
                select claim.ClaimValue;

            if (userClaims.Contains(RoleName.Admin))
            {
                StatusMessage = "Error: " + Input.UserName + " is an admin.";
                return RedirectToPage();
            }

            var userLockEnd = await _userManager.GetLockoutEndDateAsync(user);

            if (userLockEnd != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                StatusMessage = "Success: " + Input.UserName + " is now unlocked.";
                return RedirectToPage();
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(200));
                StatusMessage = "Success: " + Input.UserName + " is now blocked.";
                return RedirectToPage();
            }
        }
    }
}
