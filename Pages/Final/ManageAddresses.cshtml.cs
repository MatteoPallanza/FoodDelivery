using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Final
{
    [Authorize]
    public class ManageAddressesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageAddressesModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserId = user.Id;

            return Page();
        }
    }
}
