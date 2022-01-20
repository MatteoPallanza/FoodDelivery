using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;

namespace FoodDelivery.Pages.Admin
{
    [Authorize(Policy=PolicyName.IsAdmin)]
    public class RiderFeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RiderFeeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Fee")]
            [BindProperty]
            [Required]
            public decimal Fee { get; set; }
        }

        private void Load()
        {
             var previousFee =
                (from riderFee in _context.RiderFees
                 select riderFee.Fee).FirstOrDefault();

            Input = new InputModel
            {
                Fee = previousFee
            };
        }

        public IActionResult OnGet()
        {
            Load();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Load();
                return Page();
            }

            var previousFee =
                (from riderFee in _context.RiderFees
                 select riderFee.Fee).FirstOrDefault();

            if (Input.Fee != previousFee)
            {
               (from riderFee in _context.RiderFees
                select riderFee).FirstOrDefault().Fee = Input.Fee;

                await _context.SaveChangesAsync();

                StatusMessage = "Success: the rider fee is now " + Input.Fee + ".";
                return RedirectToPage();
            }
            else
            {
                StatusMessage = "Success: the rider fee was already " + Input.Fee + ".";
                return RedirectToPage();
            }
        }
    }
}
