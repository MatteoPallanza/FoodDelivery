using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Data
{
    public class Restaurateur
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public byte[] Logo { get; set; }


        [Key]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CategoryId { get; set; }
        public RestaurateurCategory Category { get; set; }
    }
}