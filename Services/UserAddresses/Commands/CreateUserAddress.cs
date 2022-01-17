using FoodDelivery.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace FoodDelivery.Services.UserAddresses.Commands
{
    public class CreateUserAddress : AbstractUserAddressRequest
    {
    }

    public class CreateUserAddressHandler : IRequestHandler<CreateUserAddress, UserAddress>
    {
        readonly ApplicationDbContext _context;

        public CreateUserAddressHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserAddress> Handle(CreateUserAddress request, CancellationToken cancellationToken)
        {
            var address = new UserAddress
            {
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                UserId = request.UserId
            };

            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
