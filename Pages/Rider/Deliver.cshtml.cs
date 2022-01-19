using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Rider
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class DeliverModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public DeliverModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        public int OrderId { get; set; }

        public string DeliveryAddress { get; set; }

        private void Load(int orderId)
        {
            Input = new InputModel
            {
                OrderId = orderId
            };
        }

        public IActionResult OnGet(int orderId)
        {
            OrderId = orderId;
            var order =
                (from o in _context.Orders
                 where o.Id == OrderId
                 select o).FirstOrDefault();

            if (order != null)
            {
                DeliveryAddress = order.DeliveryAddress;
            }

            Load(orderId);
            return Page();
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

            if (order.Status > 3)
            {
                StatusMessage = "Error: the order " + orderId + " has been already delivered.";
                return RedirectToPage();
            }
            else
            {
                order.Status = 4;
                await _context.SaveChangesAsync();
                return RedirectToPage("Delivered", new { orderId });
            }
        }
    }
}
