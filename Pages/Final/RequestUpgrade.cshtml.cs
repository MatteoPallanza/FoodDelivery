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

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<SelectListItem> Roles = new[]
        {
            new SelectListItem("Restaurateur", "restaurateur"),
            new SelectListItem("Rider", "rider")
        };

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Role")]
            [BindProperty]
            [Required]
            public string Role { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UpgradeRequests.Add(new UpgradeRequest
            {
                Role = Input.Role,
                User = user
            });

            await _context.SaveChangesAsync();

            StatusMessage = "Success: your request has been saved.";
            return RedirectToPage();
        }
    }
}
