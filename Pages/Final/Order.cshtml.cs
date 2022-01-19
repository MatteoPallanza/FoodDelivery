using FoodDelivery.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
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

        public string RestaurateurId { get; set; }

        public Data.Restaurateur RestauratorDetails { get; set; }

        public List<Product> Products { get; set; }

        public string Category { get; set; }

        public void OnGet(string restaurateurId)
        {
            RestaurateurId = restaurateurId;

            var restaurateur =
                (from rd in _context.Restaurateurs
                 where rd.UserId == restaurateurId
                 select rd).FirstOrDefault();

            var categories =
                (from cat in _context.RestaurateurCategories
                 select cat).ToArray();

            if (restaurateur != null)
            {
                RestauratorDetails = restaurateur;

                var category=
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
            }
        }

        public async Task<IActionResult> OnPost(string restaurateurId)
        {
            var restaurateur =
                (from rest in _context.Restaurateurs
                 where rest.UserId == restaurateurId
                 select rest).FirstOrDefault();

            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;

            string deliveryAddress = Request.Form["deliveryAddress"].FirstOrDefault();

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
                        order = new Order { Date = DateTime.Now, Status = 2, UserId = userId, RestaurateurId = restaurateurId , DeliveryAddress = deliveryAddress};
                        orderCreated = true;

                        _context.Orders.Add(order);
                        _context.SaveChanges();
                    }

                    var grossPrice =
                        (from product in products
                         where product.Id == id
                         select new { Price = product.Price, Discount = product.Discount }).FirstOrDefault();
                    var finalPrice = grossPrice.Price - grossPrice.Discount;

                    OrderDetail orderDetail = new() { OrderId = order.Id, ProductId = id, Quantity = qty , Price = finalPrice};
                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                }
            }

            return RedirectToPage("OrderCompleted", new {orderId = order.Id});
        }


    }
}
