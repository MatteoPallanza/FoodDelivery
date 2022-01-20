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
            var order =
                (from o in _context.Orders
                 where o.Id == orderId && o.Status < 4
                 select o).FirstOrDefault();

            var userId = _userManager.GetUserId(User);

            // per evitare che altri rider vedano il percorso di un ordine altrui o di ordini completati
            if ((order == null) || (userId != order.RiderId))
            {
                return NotFound("Error: no order exists with the defined ID, the order has already been marked as delivered or you aren't allowed to access this page.");
            }
            else
            {
                DeliveryAddress = order.DeliveryAddress;
                Load(orderId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
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

            // per evitare che altri rider contrassegnino come consegnato un ordine altrui
            var userId = _userManager.GetUserId(User);
            if (userId != order.RiderId)
            {
                return NotFound("Error: you aren't allowed to access this page.");
            }

            if (order.Status > 3)
            {
                StatusMessage = "Error: the order " + Input.OrderId + " has already been marked as delivered.";
                return RedirectToPage(new { orderId = Input.OrderId });
            }
            else
            {
                order.Status = 4;
                await _context.SaveChangesAsync();
                return RedirectToPage("Delivered", new { orderId = Input.OrderId });
            }
        }
    }
}
