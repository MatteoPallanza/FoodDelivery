using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Data
{
    public class Review
    {
        [Key]
        public int OrderId { get; set; }

        public Order Order { get; set; }


        public int Rating { get; set; }

        public string ReviewTitle { get; set; }

        public string ReviewText { get; set; }
    }
}
