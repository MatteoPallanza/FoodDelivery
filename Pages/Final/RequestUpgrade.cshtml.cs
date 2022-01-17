using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Final
{
    [Authorize]
    public class RequestUpgradeModel : PageModel
    {
        readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public RequestUpgradeModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<SelectListItem> Roles = new[]
        {
            new SelectListItem("Restaurateur", "restaurateur"),
            new SelectListItem("Rider", "rider")
        };

        [Display(Name = "Role")]
        [BindProperty]
        public string Role { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {

                _context.UpgradeRequests.Add(new UpgradeRequest
                {
                    Role = Role,
                    User = applicationUser
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
