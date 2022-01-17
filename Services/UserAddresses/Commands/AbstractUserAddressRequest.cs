using FoodDelivery.Data;
using MediatR;

namespace FoodDelivery.Services.UserAddresses.Commands
{
    public class AbstractUserAddressRequest : IRequest<UserAddress>
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string UserId { get; set; }
    }
}