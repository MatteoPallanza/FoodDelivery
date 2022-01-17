namespace FoodDelivery.Services.UserAddresses.Queries
{
    public class GetUserAddressesModel
    {
        public GetUserAddressesModel(int id, string address, string city, string postalCode)
        {
            Id = id;
            Address = address;
            City = city;
            PostalCode = postalCode;
        }

        public int Id { get; set; }
        public string Address { get; }
        public string City { get; }
        public string PostalCode { get; }
    }
}
