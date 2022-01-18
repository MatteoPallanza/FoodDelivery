namespace FoodDelivery.Services.UpgradeRequests.Queries
{
    public class GetUpgradeRequestsModel
    {
        public GetUpgradeRequestsModel (int id, string userName, string role)
        {
            Id = id;
            UserName = userName;
            Role = role;
        }

        public int Id { get; }
        public string UserName { get; }
        public string Role { get; }
    }
}
