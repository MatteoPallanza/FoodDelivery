using FoodDelivery.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDelivery.Services.UserAddresses.Commands
{
    public class DeleteUserAddress : AbstractUserAddressRequest
    {
    }

    public class DeleteUserAddressHandler : IRequestHandler<DeleteUserAddress, UserAddress>
    {
        readonly ApplicationDbContext _context;

        public DeleteUserAddressHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserAddress> Handle(DeleteUserAddress request, CancellationToken cancellationToken)
        {
            var address = new UserAddress
            {
                Id = request.Id,
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                UserId = request.UserId
            };

            _context.Entry(address).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
