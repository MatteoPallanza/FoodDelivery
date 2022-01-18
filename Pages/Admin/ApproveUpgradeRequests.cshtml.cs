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
    public class ApproveUpgradeRequestsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public ApproveUpgradeRequestsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var requestId = Input.RequestId;

            var requestDetails =
                (from req in _context.UpgradeRequests
                 join us in _context.Users on req.UserId equals us.Id
                 where req.Id == requestId
                 select new { UserName = us.UserName, Role = req.Role }).FirstOrDefault();

            if (requestDetails == null)
            {
                StatusMessage = "Error: no request exists with the defined ID.";
                return RedirectToPage();
            }

            var userName = requestDetails.UserName;
            var role = requestDetails.Role;

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

            var requestToDelete =
                (from req in _context.UpgradeRequests
                 where req.Id == requestId
                 select req).FirstOrDefault();

            _context.Entry(requestToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

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

            StatusMessage = "Error: unexpected error when trying process the request.";
            return RedirectToPage();
        }
    }
}
