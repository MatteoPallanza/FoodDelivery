using FoodDelivery.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using FoodDelivery.Authorization;

namespace FoodDelivery.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class RiderInfoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RiderInfoModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
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
            [Display(Name = "Credit")]
            [BindProperty]
            public decimal Credit { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);

            var deliveriesCount =
                (from o in _context.Orders
                 where o.RiderId == userId && o.Status == 4
                 select o).Count();

            var riderFees =
                (from rf in _context.RiderFees
                 select rf.Fee);

            decimal credit;

            if (riderFees != null)
            {
                credit = deliveriesCount * riderFees.FirstOrDefault();
            }
            else
            {
                StatusMessage = "Unable to retrieve rider fee";
                credit = 0;
            }

            Input = new InputModel
            {
                Credit = credit
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}
