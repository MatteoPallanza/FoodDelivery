using System.Collections.Generic;

namespace FoodDelivery.Data
{
    public class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }


        public int Id { get; set; }
        

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public byte[] Picture { get; set; }


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
