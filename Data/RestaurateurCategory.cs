using System.Collections.Generic;

namespace FoodDelivery.Data
{
    public class RestaurateurCategory
    {
        public RestaurateurCategory()
        {
            Restaurateurs = new HashSet<Restaurateur>();
        }

        public int Id { get; set; }


        public string Name { get; set; }


        public ICollection<Restaurateur> Restaurateurs { get; set; }
    }
}
