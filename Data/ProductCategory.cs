using System.Collections.Generic;

namespace FoodDelivery.Data
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }


        public int Id { get; set; }


        public string Name { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}