using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Final
{
    [Authorize]
    public class PayOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public PayOrderModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

            [Display(Name = "Total")]
            [BindProperty]
            [Required]
            public decimal Total { get; set; }
        }

        public decimal Total { get; set; }

        private void Load(int orderId)
        {
            decimal total = 0;
            var partials =
                (from od in _context.OrderDetails
                 where od.OrderId == orderId
                 select od).ToList();

            foreach (var item in partials)
            {
                total += item.Price * item.Quantity;
            }

            Input = new InputModel
            {
                OrderId = orderId,
                Total = total
            };
        }

        public IActionResult OnGet(int orderId)
        {
            var order =
                (from o in _context.Orders
                 where o.Id == orderId && o.Status < 2
                 select o).FirstOrDefault();

            var userId = _userManager.GetUserId(User);

            if ((order == null) || (userId != order.UserId))
            {
                return NotFound("Error: no order exists with the defined ID, the order has already been paid or you aren't allowed to access this page.");
            }
            else
            {
                Load(orderId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var order =
                (from o in _context.Orders
                 where o.Id == Input.OrderId
                 select o).FirstOrDefault();

            if (order == null)
            {
                StatusMessage = "Error: no order exists with the defined ID.";
                return RedirectToPage(new { orderId = Input.OrderId });
            }

            var userId = _userManager.GetUserId(User);
            if (userId != order.UserId)
            {
                return NotFound("Error: you aren't allowed to access this page.");
            }

            if (order.Status > 1)
            {
                StatusMessage = "Error: the order " + Input.OrderId + " has already been paid.";
                return RedirectToPage(new { orderId = Input.OrderId });
            }
            else
            {
                order.Status = 2;
                await _context.SaveChangesAsync();
                return RedirectToPage("OrderCompleted", new { orderId = Input.OrderId });
            }
        }
    }
}
