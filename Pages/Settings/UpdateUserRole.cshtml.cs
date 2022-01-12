using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FoodDelivery.Pages.Settings
{
    [Authorize(Policy=PolicyName.IsAdmin)]
    public class UpdateUserRoleModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserRoleModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<SelectListItem> Roles = new[]
        {
            new SelectListItem("Final", "final"),
            new SelectListItem("Restaurateur", "restaurateur"),
            new SelectListItem("Rider", "rider"),
            new SelectListItem("Admin", "admin")
        };

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Username")]
            [BindProperty]
            public string UserName { get; set; }

            [Display(Name = "Role")]
            [BindProperty]
            public string Role { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userName = Input.UserName ?? "";
            var role = Input.Role;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                StatusMessage = "Error: no user exists with the defined username.";
                return RedirectToPage();
            }

            var userClaims=
                from uc in _context.UserClaims
                where uc.UserId == user.Id
                select uc.ClaimValue;

            if (role == "final")
            {
                var allUserClaims = await _userManager.GetClaimsAsync(user);
                await _userManager.RemoveClaimsAsync(user, allUserClaims);

                StatusMessage = "Success: user " + userName + " is now a final user.";
                return RedirectToPage();
            }

            if (role == "restaurateur")
            {
                if (userClaims.Contains(RoleName.Restaurateur))
                {
                    StatusMessage = "Success: user " + userName + " is already a restaurateur.";
                    return RedirectToPage();
                }

                if (userClaims.Contains(RoleName.Rider))
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));

                    StatusMessage = "Success: user " + userName + " was a rider, now is a restaurateur.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));

                StatusMessage = "Success: user " + userName + " is now a restaurateur.";
                return RedirectToPage();
            }

            if (role == "rider")
            {
                if (userClaims.Contains(RoleName.Rider))
                {
                    StatusMessage = "Success: user " + userName + " is already a rider.";
                    return RedirectToPage();
                }

                if (userClaims.Contains(RoleName.Restaurateur))
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));

                    StatusMessage = "Success: user " + userName + " was a restaurateur, now is a rider.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));

                StatusMessage = "Success: user " + userName + " is now a rider.";
                return RedirectToPage();
            }

            if (role == "admin")
            {
                if (userClaims.Contains(RoleName.Admin))
                {
                    StatusMessage = "Success: user " + userName + " is already an admin.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Admin));

                StatusMessage = "Success: user " + userName + " is an admin.";
                return RedirectToPage();
            }

            StatusMessage = "Error: unexpected error when trying process the request.";
            return RedirectToPage();
        }
    }
}
