namespace FoodDelivery.Data
{
    public class UserAddress
    {
        public int Id { get; set; }


        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}