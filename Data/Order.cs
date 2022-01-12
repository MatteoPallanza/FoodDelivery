using System;
using System.Collections.Generic;

namespace FoodDelivery.Data
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }


        public int Id { get; set; }


        public DateTime Date { get; set; }

        public int Status { get; set; }

        public int Rating { get; set; }

        public string ReviewTitle { get; set; }

        public string Review { get; set; }


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string RestaurateurId { get; set; }
        public ApplicationUser Restaurateur { get; set; }

        public string RiderId { get; set; }
        public ApplicationUser Rider { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
