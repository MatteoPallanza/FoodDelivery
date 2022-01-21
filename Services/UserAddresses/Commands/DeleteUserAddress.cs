using AutoMapper;
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
        readonly IMapper _mapper;

        public DeleteUserAddressHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserAddress> Handle(DeleteUserAddress request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<UserAddress>(request);

            _context.Entry(address).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return address;
        }
    }
}
