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

namespace FoodDelivery.Pages.Admin
{
    [Authorize(Policy=PolicyName.IsAdmin)]
    public class UpdateRolesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateRolesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            [Required]
            public string UserName { get; set; }

            [Display(Name = "Role")]
            [BindProperty]
            [Required]
            public string Role { get; set; }
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

            var userClaims=
                from claim in _context.UserClaims
                where claim.UserId == user.Id
                select claim.ClaimValue;

            if (Input.Role == "final")
            {
                var allUserClaims = await _userManager.GetClaimsAsync(user);
                await _userManager.RemoveClaimsAsync(user, allUserClaims);

                StatusMessage = "Success: user " + Input.UserName + " is now a final user.";
                return RedirectToPage();
            }

            if (Input.Role == "restaurateur")
            {
                if (userClaims.Contains(RoleName.Restaurateur))
                {
                    StatusMessage = "Success: user " + Input.UserName + " is already a restaurateur.";
                    return RedirectToPage();
                }

                if (userClaims.Contains(RoleName.Rider))
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));

                    StatusMessage = "Success: user " + Input.UserName + " was a rider, now is a restaurateur.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));

                StatusMessage = "Success: user " + Input.UserName + " is now a restaurateur.";
                return RedirectToPage();
            }

            if (Input.Role == "rider")
            {
                if (userClaims.Contains(RoleName.Rider))
                {
                    StatusMessage = "Success: user " + Input.UserName + " is already a rider.";
                    return RedirectToPage();
                }

                if (userClaims.Contains(RoleName.Restaurateur))
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));

                    StatusMessage = "Success: user " + Input.UserName + " was a restaurateur, now is a rider.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));

                StatusMessage = "Success: user " + Input.UserName + " is now a rider.";
                return RedirectToPage();
            }

            if (Input.Role == "admin")
            {
                if (userClaims.Contains(RoleName.Admin))
                {
                    StatusMessage = "Success: user " + Input.UserName + " is already an admin.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Admin));

                StatusMessage = "Success: user " + Input.UserName + " is an admin.";
                return RedirectToPage();
            }

            StatusMessage = "Error: unexpected error when trying process the request.";
            return RedirectToPage();
        }
    }
}
