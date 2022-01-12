using FoodDelivery.Authorization;
using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy=PolicyName.IsRestaurateur)]
    public class RestaurateurInfoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RestaurateurInfoModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context; 
            _userManager = userManager;
        }

        public List<SelectListItem> Categories = new();

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Name")]
            [BindProperty]
            public string Name { get; set; }

            [Display(Name = "Category")]
            [BindProperty]
            public string Category { get; set; }

            [Display(Name = "Address")]
            [BindProperty]
            public string Address { get; set; }

            [Display(Name = "City")]
            [BindProperty]
            public string City { get; set; }

            [Display(Name = "Postal Code")]
            [BindProperty]
            public string PostalCode { get; set; }

            [Display(Name = "Logo")]
            [BindProperty]
            public byte[] Logo { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var restaurateur = 
                (from r in _context.Restaurateurs
                where r.UserId == userId
                select r).FirstOrDefault();

            var categoriesList =
                from cat in _context.RestaurateurCategories
                select cat;

            foreach (var category in categoriesList)
            {
                Categories.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            }

            if (restaurateur != null)
            {
                var name = restaurateur.Name;
                var category = restaurateur.Category.Id.ToString();
                var address = restaurateur.Address;
                var city = restaurateur.City;
                var postalCode = restaurateur.PostalCode;
                var logo = restaurateur.Logo;

                Input = new InputModel
                {
                    Name = name,
                    Category = category,
                    Address = address,
                    City = city,
                    PostalCode = postalCode,
                    Logo = logo
                };
            }
            else
            {
                Input = new InputModel
                {
                    Logo = null
                };
            }
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var restaurateur =
                (from r in _context.Restaurateurs
                 where r.UserId == userId
                 select r).FirstOrDefault();

            if (restaurateur == null)
            {
                restaurateur = new Restaurateur { UserId = userId };
                _context.Restaurateurs.AddRange(restaurateur);
            }

            if (Input.Name != restaurateur.Name)
            {
                restaurateur.Name = Input.Name;
            }

            if (Input.Address != restaurateur.Address)
            {
                restaurateur.Address = Input.Address;
            }

            if (Input.City != restaurateur.City)
            {
                restaurateur.City = Input.City;
            }

            if (Input.PostalCode != restaurateur.PostalCode)
            {
                restaurateur.PostalCode = Input.PostalCode;
            }

            
            if (Input.Category != restaurateur.CategoryId.ToString())
            {
                 var conversionSuccess = int.TryParse(Input.Category, out int categoryId);

                if (conversionSuccess)
                {
                    restaurateur.CategoryId = categoryId;
                }
                else
                {
                    StatusMessage = "Unexpected error when trying to set category.";
                    return RedirectToPage();
                }
            }
            
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    restaurateur.Logo = dataStream.ToArray();
                }
            }
            
            await _context.SaveChangesAsync();

            StatusMessage = "Your restaurateur profile has been updated";
            return RedirectToPage();
        }
    }
}
