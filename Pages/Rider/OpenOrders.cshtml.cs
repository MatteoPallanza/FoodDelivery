using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Pages.Rider
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class OpenOrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public OpenOrdersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
            [Display(Name = "Order ID")]
            [BindProperty]
            [Required]
            public int OrderId { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var orderId = Input.OrderId;

            var order =
                (from o in _context.Orders
                 where o.Id == orderId
                 select o).FirstOrDefault();

            if (order == null)
            {
                StatusMessage = "Error: no order exists with the defined ID.";
                return RedirectToPage();
            }

            if (order.Status > 2)
            {
                StatusMessage = "Error: the order " + orderId + " has been already picked up.";
                return RedirectToPage();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                var riderId = user.Id;

                order.Status = 3;
                order.RiderId = riderId;
                await _context.SaveChangesAsync();
                return RedirectToPage("Deliver", new { orderId });
            }
        }
    }
}
