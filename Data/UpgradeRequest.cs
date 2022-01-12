namespace FoodDelivery.Data
{
    public class UpgradeRequest
    {
        public int Id { get; set; }


        public string Role { get; set; }


        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
