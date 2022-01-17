using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.UserAddresses.Commands
{
    public class UpdateUserAddress : AbstractUserAddressRequest
    {
    }

    public class UpdateUserAddressHandler : IRequestHandler<UpdateUserAddress, UserAddress>
    {
        readonly ApplicationDbContext _context;

        public UpdateUserAddressHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserAddress> Handle(UpdateUserAddress request, CancellationToken cancellationToken)
        {
            var address = new UserAddress
            {
                Id = request.Id,
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                UserId = request.UserId
            };

            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
