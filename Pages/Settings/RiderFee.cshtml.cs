using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;

namespace FoodDelivery.Pages.Settings
{
    [Authorize(Policy=PolicyName.IsAdmin)]
    public class RiderFeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RiderFeeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Fee")]
            [BindProperty]
            public decimal Fee { get; set; }
        }

        private decimal PreviousFee { get; set; }

        private void Load()
        {
            PreviousFee =
                (from rf in _context.RiderFees
                 select rf.Fee).FirstOrDefault();

            Input = new InputModel
            {
                Fee = PreviousFee
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

            if (Input.Fee != PreviousFee)
            {
                (from rf in _context.RiderFees
                 select rf).FirstOrDefault().Fee = Input.Fee;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
