using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Admin
{
    [Authorize(Policy = PolicyName.IsAdmin)]
    public class ApproveModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public ApproveModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            [Display(Name = "Request ID")]
            [BindProperty]
            [Required]
            public int RequestId { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var requestDetails =
                (from request in _context.UpgradeRequests
                 join usr in _context.Users on request.UserId equals usr.Id
                 where request.Id == Input.RequestId
                 select new { UserName = usr.UserName, Role = request.Role }).FirstOrDefault();

            if (requestDetails == null)
            {
                StatusMessage = "Error: no request exists with the defined ID.";
                return RedirectToPage();
            }

            var user = await _userManager.FindByNameAsync(requestDetails.UserName);

            if (user == null)
            {
                StatusMessage = "Error: no user exists with the defined username.";
                return RedirectToPage();
            }

            var userClaims =
                from claim in _context.UserClaims
                where claim.UserId == user.Id
                select claim.ClaimValue;

            var requestToDelete =
                (from req in _context.UpgradeRequests
                 where req.Id == Input.RequestId
                 select req).FirstOrDefault();

            _context.Entry(requestToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            if (requestDetails.Role == "restaurateur")
            {
                if (userClaims.Contains(RoleName.Restaurateur))
                {
                    StatusMessage = "Success: user " + requestDetails.UserName + " is already a restaurateur.";
                    return RedirectToPage();
                }

                if (userClaims.Contains(RoleName.Rider))
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));

                    StatusMessage = "Success: user " + requestDetails.UserName + " was a rider, now is a restaurateur.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));

                StatusMessage = "Success: user " + requestDetails.UserName + " is now a restaurateur.";
                return RedirectToPage();
            }

            if (requestDetails.Role == "rider")
            {
                if (userClaims.Contains(RoleName.Rider))
                {
                    StatusMessage = "Success: user " + requestDetails.UserName + " is already a rider.";
                    return RedirectToPage();
                }

                if (userClaims.Contains(RoleName.Restaurateur))
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(ClaimName.Role, RoleName.Restaurateur));
                    await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));

                    StatusMessage = "Success: user " + requestDetails.UserName + " was a restaurateur, now is a rider.";
                    return RedirectToPage();
                }

                await _userManager.AddClaimAsync(user, new Claim(ClaimName.Role, RoleName.Rider));

                StatusMessage = "Success: user " + requestDetails.UserName + " is now a rider.";
                return RedirectToPage();
            }

            StatusMessage = "Error: unexpected error when trying process the request.";
            return RedirectToPage();
        }
    }
}
