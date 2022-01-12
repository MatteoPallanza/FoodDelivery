using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FoodDelivery.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserAddresses = new HashSet<UserAddress>();
            UpgradeRequests = new HashSet<UpgradeRequest>();
            Products = new HashSet<Product>();
            UserOrders = new HashSet<Order>();
            RestaurateurOrders = new HashSet<Order>();
            RiderOrders = new HashSet<Order>();
        }


        public string Name { get; set; }

        public string Surname { get; set; }

        public ICollection<UserAddress> UserAddresses { get; set; }

        public ICollection<UpgradeRequest> UpgradeRequests { get; set; }

        public Restaurateur RestaurateurDetail { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Order> UserOrders { get; set; }

        public ICollection<Order> RestaurateurOrders { get; set; }

        public ICollection<Order> RiderOrders { get; set; }
    }
}
