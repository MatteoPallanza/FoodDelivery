using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Pages.Final
{
    [Authorize]
    public class ReviewsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ReviewsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<SelectListItem> Ratings = new[]
        {
            new SelectListItem("1 Star", "1"),
            new SelectListItem("2 Stars", "2"),
            new SelectListItem("3 Stars", "3"),
            new SelectListItem("4 Stars", "4"),
            new SelectListItem("5 Stars", "5")
        };

        public List<SelectListItem> OrderIds = new();

        public string UserId { get; set; }

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

            [Display(Name = "Rating")]
            [BindProperty]
            [Required]
            public int Rating { get; set; }

            [Display(Name = "Title")]
            [BindProperty]
            [Required]
            public string ReviewTitle { get; set; }

            [Display(Name = "Review")]
            [BindProperty]
            [StringLength(250)]
            public string ReviewText { get; set; }
        }

        private void LoadAsync(ApplicationUser user)
        {
            var completedOrders =
                (from o in _context.Orders
                 where o.UserId == user.Id && o.Status == 4
                 select o.Id);

            foreach (var order in completedOrders)
            {
                OrderIds.Add(new SelectListItem() { Text = order.ToString(), Value = order.ToString() });
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserId = user.Id;

            LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                LoadAsync(user);
                return Page();
            }

            var review = new Review()
            {
                OrderId = Input.OrderId,
                Rating = Input.Rating,
                ReviewTitle = Input.ReviewTitle,
                ReviewText = Input.ReviewText
            };

            bool alreadyReviewed =
                (from r in _context.Reviews
                 where r.OrderId == Input.OrderId
                 select r).Any();

            if (!alreadyReviewed)
            {
                _context.Reviews.Add(review);
                StatusMessage = "Success: your review for order " + Input.OrderId + " has been saved.";
            }
            else
            {
                _context.Entry(review).State = EntityState.Modified;
                StatusMessage = "Success: your review for order " + Input.OrderId + " has been updated.";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
