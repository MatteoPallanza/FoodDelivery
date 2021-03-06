using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Rider
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class CompletedDeliveriesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CompletedDeliveriesModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserName = user.UserName;

            return Page();
        }
    }
}
