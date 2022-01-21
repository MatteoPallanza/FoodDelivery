using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Final
{
    [Authorize]
    public class OrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public OrderModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Data.Restaurateur Restaurator { get; set; }

        public string Category { get; set; }

        public List<Product> Products { get; set; }

        public List<SelectListItem> DeliveryAddresses = new();

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Delivery Address")]
            [BindProperty]
            [Required]
            public string DeliveryAddress { get; set; }
        }

        private void Load(ApplicationUser user)
        {
            var deliveryAddresses =
                (from a in _context.UserAddresses
                 where a.UserId == user.Id
                 select a);

            foreach (var address in deliveryAddresses)
            {
                var addressString = address.Address.ToString() + ", " + address.PostalCode.ToString() + " " + address.City.ToString();

                DeliveryAddresses.Add(new SelectListItem() { Text = addressString, Value = addressString });
            }
        }

        public async Task<IActionResult> OnGetAsync(string restaurateurId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var restaurateur =
                (from rd in _context.Restaurateurs
                 where rd.UserId == restaurateurId
                 select rd).FirstOrDefault();

            if (restaurateur == null)
            {
                return NotFound("Error: unable to load restaurator with ID " + restaurateurId + ".");
            }

            Restaurator = restaurateur;

            var category =
                (from cat in _context.RestaurateurCategories
                 where cat.Id == restaurateur.CategoryId
                 select cat.Name).FirstOrDefault();

            if (category != null)
            {
                Category = category;
            }

            var products =
                (from prod in _context.Products
                 where prod.UserId == restaurateurId
                 select prod).ToList();

            Products = products;

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string restaurateurId)
        {
            var restaurateur =
                (from rest in _context.Restaurateurs
                 where rest.UserId == restaurateurId
                 select rest).FirstOrDefault();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            string deliveryAddress = Input.DeliveryAddress;

            var products =
                (from product in _context.Products
                 where product.UserId == restaurateurId
                 select product).ToList();

            var productIds =
                (from product in products
                 select product.Id).ToList();

            bool orderCreated = false;
            Order order = null;

            foreach (var id in productIds)
            {
                int.TryParse(Request.Form["prod_" + id].FirstOrDefault(), out int qty);

                if (qty > 0)
                {
                    if (!orderCreated)
                    {
                        order = new Order { Date = DateTime.Now, Status = 1, UserId = user.Id, RestaurateurId = restaurateurId , DeliveryAddress = deliveryAddress};
                        orderCreated = true;

                        _context.Orders.Add(order);
                        _context.SaveChanges();
                    }

                    var price =
                        (from product in products
                         where product.Id == id
                         select new { Price = product.Price, Discount = product.Discount }).FirstOrDefault();
                    var finalPrice = price.Price - price.Discount;

                    OrderDetail orderDetail = new() { OrderId = order.Id, ProductId = id, Quantity = qty , Price = finalPrice};
                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                }
            }

            return RedirectToPage("PayOrder", new {orderId = order.Id});
        }
    }
}
